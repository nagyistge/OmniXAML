namespace OmniXaml.Tests.ObjectAssemblerTests.New
{
    using Testing.Classes;
    using Xunit;

    public class Collections
    {
        public Collections()
        {
            Fixture = new ObjectAssemblerFixtureNew();
        }

        public ObjectAssemblerFixtureBase Fixture { get; set; }

        [Fact]
        public void Collection()
        {
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.CollectionWithMoreThanOneItemNewAge);

            var result = sut.Result;
            var children = ((DummyClass)result).Items;

            Assert.IsType(typeof(DummyClass), result);
            Assert.Equal(3, children.Count);
            Assert.All(children, child => Assert.IsType(typeof(Item), child));
        }
    }
}