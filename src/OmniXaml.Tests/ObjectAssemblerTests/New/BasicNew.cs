namespace OmniXaml.Tests.ObjectAssemblerTests.New
{
    using Testing.Classes;
    using Xunit;

    public class BasicNew : Basic
    {
        public BasicNew()
        {
            Fixture = new PureObjectAssemblerFixture();
        }        
    }
}