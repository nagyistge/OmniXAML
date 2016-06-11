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

            if (instruction.InstructionType == InstructionType.StartMember)
            {
                workbenches.CurrentValue.Member = instruction.Member;
            }

            if (instruction.InstructionType == InstructionType.Value)
            {
                workbenches.CurrentValue.SetMemberValue(instruction.Value);
            }
        }

        public void OverrideInstance(object instance)
        {
            throw new NotImplementedException();
        }
    }

    internal class Workbench
    {
        private readonly IValueContext valueContext;

        public Workbench(IValueContext valueContext)
        {
            this.valueContext = valueContext;
        }

        public object Instance { get; set; }
        public MemberBase Member { get; set; }

        public void SetMemberValue(object value)
        {
            var mutable = Member as MutableMember;
            mutable.SetValue(Instance, value, valueContext);
        }
    }
}