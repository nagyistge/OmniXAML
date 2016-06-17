namespace OmniXaml.Tests.ObjectAssemblerTests
{
    using New;

    public class InstanceNamingNew
    {
        public InstanceNamingNew()
        {
            Fixture = new PureObjectAssemblerFixture();
        }

        public PureObjectAssemblerFixture Fixture { get; set; }
    }
}