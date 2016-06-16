namespace OmniXaml.Tests.ObjectAssemblerTests
{
    using New;
    using Xunit;

    public class InitializationNew : Initialization
    {
        public InitializationNew()
        {
            Fixture = new PureObjectAssemblerFixture();
        }
    }
}