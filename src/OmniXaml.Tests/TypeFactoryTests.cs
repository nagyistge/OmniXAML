namespace OmniXaml.Tests
{
    using Testing.Classes;
    using Xunit;

    public class TypeFactoryTests
    {
        [Fact]
        public void NoParameters()
        {
            var sut = new TypeFactory();

            Assert.Null(sut.Create(typeof(ImmutableDummy)));
        }

        [Fact]
        public void OnlyOneParameter()
        {
            var sut = new TypeFactory();

            Assert.NotNull(sut.Create(typeof(ImmutableDummy), "some string"));
        }

        [Fact]
        public void TwoParameters()
        {
            var sut = new TypeFactory();

            Assert.NotNull(sut.Create(typeof(ImmutableDummy), "some string", new ChildClass()));
        }

        [Fact]
        public void TwoParametersReversedOrder()
        {
            var sut = new TypeFactory();

            Assert.NotNull(sut.Create(typeof(ImmutableDummy), new ChildClass(), "some string"));
        }

        [Fact]
        public void TwoParametersReversedOrderWithDerivedChild()
        {
            var sut = new TypeFactory();

            Assert.NotNull(sut.Create(typeof(ImmutableDummy), new DerivedChild(), "some string"));
        }

        [Fact]
        public void TwoParametersUnmatchingTypes()
        {
            var sut = new TypeFactory();

            Assert.Null(sut.Create(typeof(ImmutableDummy), "one", "two"));
        }
    }
}
