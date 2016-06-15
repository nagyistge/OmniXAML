namespace OmniXaml.Tests.ObjectAssemblerTests.New
{
    using System;
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
                    workbenches.CurrentValue.Flag = true;
                    break;
                case InstructionType.EndMember:
                    var instanceToAssign = workbenches.CurrentValue.Instance;
                    workbenches.Pop();
                    workbenches.CurrentValue.SetMemberValue(instanceToAssign);
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
                        }
                    }

                    result = workbenches.CurrentValue.Instance;
                    //workbenches.Pop();

                    break;
            }
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