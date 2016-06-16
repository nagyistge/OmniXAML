namespace OmniXaml.Tests.ObjectAssemblerTests.New
{
    using Testing.Classes;
    using Xunit;

    public class CollectionsNew : Collections
    {
        public CollectionsNew()
        {
            Fixture = new PureObjectAssemblerFixture();
        }

        [Fact]
        public void ImmutableCollection()
        {
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.ImmutableCollectionWithMoreThanOneItem);

            var result = sut.Result;
            var children = ((DummyClass)result).ImmutableItems;

            Assert.IsType(typeof(DummyClass), result);
            Assert.Equal(3, children.Count);
            Assert.All(children, child => Assert.IsType(typeof(Item), child));
        }
    }
}