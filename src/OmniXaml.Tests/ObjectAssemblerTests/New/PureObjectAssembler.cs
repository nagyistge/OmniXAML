namespace OmniXaml.Tests.ObjectAssemblerTests.New
{
    using System;
    using Glass.Core;
    using ObjectAssembler;
    using ObjectAssembler.Commands;
    using TypeConversion;

    public class PureObjectAssembler : IObjectAssembler
    {
        private readonly IValueContext valueContext;

        public PureObjectAssembler(IValueContext valueContext)
        {
            this.valueContext = valueContext;
        }

        private object result;
        private readonly StackingLinkedList<Workbench> workbenches = new StackingLinkedList<Workbench>();

        public object Result => workbenches.CurrentValue.Instance;
        public EventHandler<XamlSetValueEventArgs> XamlSetValueHandler { get; set; }
        public IRuntimeTypeSource TypeSource { get; }
        public ITopDownValueContext TopDownValueContext { get; }
        public IInstanceLifeCycleListener LifecycleListener { get; }
        public void Process(Instruction instruction)
        {
            if (instruction.InstructionType == InstructionType.StartObject)
            {
                var workbench = new Workbench(valueContext);
                workbench.Instance = instruction.XamlType.CreateInstance(null);
                workbenches.Push(workbench);
            }

            if (instruction.InstructionType == InstructionType.EndMember)
            {
                var instanceToAssign = workbenches.CurrentValue.Instance;
                workbenches.Pop();
                workbenches.CurrentValue.SetMemberValue(instanceToAssign);
            }

            if (instruction.InstructionType == InstructionType.StartMember)
            {
                workbenches.CurrentValue.Member = instruction.Member;
            }

            if (instruction.InstructionType == InstructionType.Value)
            {
                var workbench = new Workbench(valueContext);
                workbench.Instance = instruction.Value;
                workbenches.Push(workbench);
            }
        }

        public void OverrideInstance(object instance)
        {
            throw new NotImplementedException();
        }
    }
}