namespace OmniXaml.Tests.ObjectFactories
{
    using OmniXaml.ObjectFactories;
    using Testing.Classes;
    using Xunit;

    public class NameMatchingTypeFactoryTests
    {     
        [Fact]
        public void OnlyOneParameterMatchingByName()
        {
            var withInjection = (ImmutableDummy)CreateSut().Create(typeof(ImmutableDummy), new InjectableValue("Text", "Some string"));
            Assert.NotNull(withInjection);
            Assert.Equal("Some string", withInjection.Text);
        }

        [Fact]
        public void TwoParameterRightOrderNoDerivedTypes()
        {
            var withInjection = (ImmutableDummy)CreateSut().Create(typeof(ImmutableDummy), new InjectableValue("Text", "Some string"), new InjectableValue("Child", new ChildClass()));
            Assert.NotNull(withInjection);
            Assert.Equal("Some string", withInjection.Text);
        }

        [Fact]
        public void TwoParameterRightOrderDerivedType()
        {
            var withInjection = (ImmutableDummy)CreateSut().Create(typeof(ImmutableDummy), new InjectableValue("Text", "Some string"), new InjectableValue("Child", new DerivedChild()));
            Assert.NotNull(withInjection);
            Assert.Equal("Some string", withInjection.Text);
        }

        private static IObjectFactory CreateSut()
        {
            return new NameMatchingObjectFactory();
        }
    }
}
