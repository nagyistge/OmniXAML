namespace OmniXaml.Tests.ObjectFactories
{
    using OmniXaml.ObjectFactories;
    using Testing.Classes;
    using Xunit;

    public class TypeMatchingTypeFactoryTests
    {
        private IObjectFactory CreateSut()
        {
            return new TypeMatchingObjectFactory();
        }

        [Fact]
        public void NoParameters()
        {
            Assert.Null(CreateSut().Create(typeof(ImmutableDummy)));
        }

        [Fact]
        public void OnlyOneParameter()
        {
            Assert.NotNull(CreateSut().Create(typeof(ImmutableDummy), new InjectableValue("some string")));
        }

        [Fact]
        public void TwoParametersOfDifferentTypes()
        {
            Assert.NotNull(CreateSut().Create(typeof(ImmutableDummy), new InjectableValue("some string"), new InjectableValue(new ChildClass())));
        }

        [Fact]
        public void TwoParametersOfSameTypes()
        {
            Assert.Null(CreateSut().Create(typeof(ImmutableDummy), new InjectableValue("some string"), new InjectableValue("some string")));
        }

        [Fact]
        public void TwoParametersReversedOrder()
        {
            Assert.NotNull(CreateSut().Create(typeof(ImmutableDummy), new InjectableValue(new ChildClass()), new InjectableValue("some string")));
        }

        [Fact]
        public void TwoParametersReversedOrderWithDerivedChild()
        {
            Assert.NotNull(CreateSut().Create(typeof(ImmutableDummy), new InjectableValue(new DerivedChild()), new InjectableValue("some string")));
        }

        [Fact]
        public void TwoParametersUnmatchingTypes()
        {
            Assert.Null(CreateSut().Create(typeof(ImmutableDummy), new InjectableValue("one"), new InjectableValue("two")));
        }
    }
}