namespace OmniXaml.Pure
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

        private readonly StackingLinkedList<Workbench> workbenches = new StackingLinkedList<Workbench>();
        private readonly IWorkshop workshop;

        public PureObjectAssembler(IValueContext valueContext)
        {
            this.valueContext = valueContext;
            workshop = new WorkshopProxy(new Workshop(valueContext, r => Result = r));
        }

        public object Result { get; private set; }

        public EventHandler<XamlSetValueEventArgs> XamlSetValueHandler { get; set; }
        public IRuntimeTypeSource TypeSource { get; }
        public ITopDownValueContext TopDownValueContext { get; }
        public IInstanceLifeCycleListener LifecycleListener { get; }

        public void Process(Instruction instruction)
        {
            switch (instruction.InstructionType)
            {
                case InstructionType.StartObject:

                    workshop.StartObject(instruction.XamlType);

                    break;

                case InstructionType.EndMember:

                    workshop.EndMember();

                    break;

                case InstructionType.StartMember:

                    workshop.StartMember(instruction.Member);
                    
                    break;

                case InstructionType.Value:

                    workshop.SetValue(instruction.Value);

                    break;

                case InstructionType.EndObject:

                    workshop.EndObject();

                    break;
            }
        }

        public void OverrideInstance(object instance)
        {
            throw new NotImplementedException();
        }
    }
}