namespace OmniXaml.Tests.ObjectAssemblerTests.New
{
    using System;
    using Glass.Core;
    using ObjectAssembler;
    using ObjectAssembler.Commands;

    public class PureObjectAssembler : IObjectAssembler
    {
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
                var workbench = new Workbench();
                workbench.Instance = instruction.XamlType.CreateInstance(null);
                workbenches.Push(workbench);
            }

            if (instruction.InstructionType == InstructionType.StartMember)
            {
                result = instruction.XamlType.CreateInstance(null);
            }
        }

        public void OverrideInstance(object instance)
        {
            throw new NotImplementedException();
        }
    }

    internal class Workbench
    {
        public object Instance { get; set; }
    }
}