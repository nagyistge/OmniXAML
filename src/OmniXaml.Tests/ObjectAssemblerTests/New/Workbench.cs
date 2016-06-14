namespace OmniXaml.Tests.ObjectAssemblerTests.New
{
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

        public void SetMemberValue(object value)
        {
            var mutable = Member as MutableMember;
            mutable.SetValue(Instance, value, valueContext);
        }
    }
}