namespace OmniXaml.Pure
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Reflection;
    using Glass.Core;
    using ObjectAssembler;
    using ObjectAssembler.Commands;
    using TypeConversion;
    using Typing;

    public class PureObjectAssembler : IObjectAssembler
    {
        private readonly IValueContext valueContext;

        public PureObjectAssembler(IValueContext valueContext)
        {
            this.valueContext = valueContext;
        }

        private readonly StackingLinkedList<Workbench> workbenches = new StackingLinkedList<Workbench>();
        private object result;

        public object Result => result;
        public EventHandler<XamlSetValueEventArgs> XamlSetValueHandler { get; set; }
        public IRuntimeTypeSource TypeSource { get; }
        public ITopDownValueContext TopDownValueContext { get; }
        public IInstanceLifeCycleListener LifecycleListener { get; }
        public void Process(Instruction instruction)
        {
            switch (instruction.InstructionType)
            {
                case InstructionType.StartObject:
                    var workbench = new Workbench(valueContext);
                    workbench.XamlType = instruction.XamlType;
                    workbenches.Push(workbench);
                    break;
                case InstructionType.EndMember:

                    if (!workbenches.CurrentValue.Flag)
                    {
                        object instanceToAssign;
                        if (Equals(workbenches.CurrentValue.Member, CoreTypes.Items))
                        {
                            instanceToAssign = CreateCollection();
                        }
                        else
                        {
                            instanceToAssign = workbenches.CurrentValue.Instance;
                        }

                        if (Equals(workbenches.CurrentValue.Member, CoreTypes.Items))
                        {
                            workbenches.PreviousValue.Flag = true;
                        }

                        workbenches.Pop();

                        object converted;
                        if (CommonValueConversion.TryConvert(instanceToAssign, workbenches.CurrentValue.Member.XamlType, valueContext, out converted))
                        {
                            workbenches.CurrentValue.SetMemberValue(converted);
                        }
                        else
                        {
                            workbenches.CurrentValue.SetMemberValue(instanceToAssign);
                        }
                    }

                    break;
                case InstructionType.StartMember:

                    if (Equals(instruction.Member, CoreTypes.Items) || Equals(instruction.Member, CoreTypes.Initialization))
                    {
                        workbenches.Push(new Workbench(valueContext));
                    }
                    else
                    {
                        workbenches.CurrentValue.Instance = workbenches.CurrentValue.XamlType.CreateInstance(null);
                    }

                    
                    workbenches.CurrentValue.Member = instruction.Member;

                    break;
                case InstructionType.Value:
                    var item = new Workbench(valueContext);
                    item.Instance = instruction.Value;
                    workbenches.Push(item);
                    break;
                case InstructionType.EndObject:
                    if (workbenches.Previous != null)
                    {
                        if (Equals(workbenches.PreviousValue.Member, CoreTypes.Items))
                        {
                            if (workbenches.CurrentValue.Instance == null)
                            {
                                workbenches.CurrentValue.Instance = workbenches.CurrentValue.XamlType.CreateInstance(null);
                            }

                            workbenches.PreviousValue.BufferedChildren.Add(workbenches.CurrentValue.Instance);
                            workbenches.Pop();
                        }
                    }

                    result = workbenches.CurrentValue.Instance;
                    
                    break;
            }
        }

        private IList CreateCollection()
        {
            var collectionType = workbenches.PreviousValue.Member.XamlType;

            if (!IsImmutable(collectionType))
            {
                var collection = (IList) collectionType.CreateInstance();
                foreach (var bufferedChild in workbenches.CurrentValue.BufferedChildren)
                {
                    collection.Add(bufferedChild);
                }
                return collection;
            }
            else
            {
                var underlyingType = workbenches.PreviousValue.Member.XamlType.UnderlyingType.GetTypeInfo().GetGenericArguments().First();

                return (IList) workbenches.CurrentValue.BufferedChildren.AsImmutable(underlyingType);
            }            
        }

        private static bool IsImmutable(XamlType collectionType)
        {
            var underlyingType = collectionType.UnderlyingType;
            if (underlyingType == null)
                return false;

            var ti = underlyingType.GetTypeInfo();
            if (!ti.IsGenericType || ti.Assembly.GetName().Name != "System.Collections.Immutable")
                return false;

            var typeDef = ti.GetGenericTypeDefinition();
            var name = typeDef.FullName;
            if (!name.StartsWith("System.Collections.Immutable.Immutable", StringComparison.Ordinal))
                return false;

            return name.EndsWith("`1", StringComparison.Ordinal) || name.EndsWith("`2", StringComparison.Ordinal);
        }

        public void OverrideInstance(object instance)
        {
            throw new NotImplementedException();
        }
    }
}