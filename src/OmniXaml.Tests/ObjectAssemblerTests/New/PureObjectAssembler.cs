namespace OmniXaml.Tests.ObjectAssemblerTests.New
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Immutable;
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
                    PushWorkbenchAndSetInstance(instruction.XamlType.CreateInstance(null));
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
                    if (Equals(instruction.Member, CoreTypes.Items))
                    {
                        workbenches.Push(new Workbench(valueContext));
                    }

                    workbenches.CurrentValue.Member = instruction.Member;

                    break;
                case InstructionType.Value:
                    PushWorkbenchAndSetInstance(instruction.Value);
                    break;
                case InstructionType.EndObject:
                    if (workbenches.Previous != null)
                    {
                        if (Equals(workbenches.PreviousValue.Member, CoreTypes.Items))
                        {
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

        private object CreateImmutableList(XamlType collectionType)
        {
            var field = collectionType.UnderlyingType.GetTypeInfo().GetField("Empty");
            var method = collectionType.UnderlyingType.GetTypeInfo().GetMethod("AddRange");
            
            var value = field.GetValue(null);
            dynamic bufferedChildren = workbenches.CurrentValue.BufferedChildren;
            var makeGenericType = typeof(IEnumerable<>).MakeGenericType(collectionType.UnderlyingType.GetGenericArguments().First());
            var a = Cast(bufferedChildren, makeGenericType);
            return method.Invoke(value, new object[]{ a });
        }
        
        public static dynamic Cast(dynamic obj, Type castTo)
        {
            return Convert.ChangeType(obj, castTo);
        }

        private bool IsImmutable(XamlType collectionType)
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

        private void PushWorkbenchAndSetInstance(object instance)
        {
            var workbench = new Workbench(valueContext);
            workbench.Instance = instance;
            workbenches.Push(workbench);
        }

        public void OverrideInstance(object instance)
        {
            throw new NotImplementedException();
        }
    }
}