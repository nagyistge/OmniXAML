using System.Collections;
using Xunit;

namespace OmniXaml.Tests.ObjectAssemblerTests.New
{
    public class InitializationNew
    {
        public InitializationNew()
        {
            Fixture = new PureObjectAssemblerFixture();
        }

        public PureObjectAssemblerFixture Fixture { get; set; }

        [Fact]
        public void IntValue()
        {
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.IntValue);
            Assert.Equal(1, sut.Result);
        }
    }
}