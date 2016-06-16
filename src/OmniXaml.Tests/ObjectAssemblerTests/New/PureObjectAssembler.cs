namespace OmniXaml.Tests.ObjectAssemblerTests.New
{
    using System;
    using System.Collections;
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
            var collection = (IList)collectionType.CreateInstance();
            foreach (var bufferedChild in workbenches.CurrentValue.BufferedChildren)
            {
                collection.Add(bufferedChild);
            }
            return collection;
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