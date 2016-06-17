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

        private readonly StackingLinkedList<Workbench> workbenches = new StackingLinkedList<Workbench>();
        private readonly Workshop workshop;

        public PureObjectAssembler(IValueContext valueContext)
        {
            this.valueContext = valueContext;
            workshop = new Workshop(valueContext, r => Result = r);
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

                    workshop.StartObjectOftype(instruction.XamlType);

                    break;

                case InstructionType.EndMember:

                    workshop.EndMember();

                    break;

                case InstructionType.StartMember:

                    if (instruction.Member.IsDirective)
                    {
                        workshop.StartDirective((Directive) instruction.Member);
                    }
                    else
                    {
                        workshop.StartMember(instruction.Member);
                    }

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