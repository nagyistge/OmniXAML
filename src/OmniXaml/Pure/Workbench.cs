namespace OmniXaml.Pure
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using TypeConversion;
    using Typing;

    internal class Workbench
    {
        private readonly IValueContext valueContext;

        public Workbench(IValueContext valueContext)
        {
            this.valueContext = valueContext;
        }

        public object Instance { get; set; }
        public MemberBase Member { get; set; }
        public ArrayList BufferedChildren { get; set; } = new ArrayList();
        public bool Flag { get; set; }
        public XamlType XamlType { get; set; }
        public IDictionary<MutableMember, object> MemberAssignments { get; } = new Dictionary<MutableMember, object>();
        public ICollection<object> InitializationValues { get; } = new Collection<object>();
        public bool IsDirectiveProcessed { get; set; }

        public void SetMemberValue(object value)
        {
            var mutable = Member as MutableMember;
            mutable.SetValue(Instance, value, valueContext);
        }

        public void CreateInstanceIfNotYetCreated()
        {
            if (Instance == null)
            {
                Instance = XamlType.CreateInstance(null);
            }
        }
    }
}