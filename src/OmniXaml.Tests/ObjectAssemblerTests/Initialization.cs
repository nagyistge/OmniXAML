namespace OmniXaml.Tests.ObjectAssemblerTests
{
    using System.Collections;
    using Xunit;

    public class Initialization
    {
        public Initialization()
        {
            Fixture = new ObjectAssemblerFixture();
        }

        protected ObjectAssemblerFixtureBase Fixture { get; set; }

        [Fact]
        public void String()
        {
            var sysNs = new NamespaceDeclaration("clr-namespace:System;assembly=mscorlib", "sys");

            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.StringInitialization(sysNs));

            var actual = sut.Result;
            Assert.IsType(typeof(string), actual);
            Assert.Equal("Text", actual);
        }

        [Fact]
        public void CollectionWithInitialization()
        {
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.PureCollection);
            var actual = (ArrayList)sut.Result;
            Assert.NotEmpty(actual);
        }
    }
}