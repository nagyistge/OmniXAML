namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using OmniXaml.Testing.Classes;
    using OmniXaml.Testing.Common;
    using Xunit;

    public abstract class SpecialTests : GivenARuntimeTypeSource
    {
        [Fact]
        public void LoadWithRootInstance()
        {
            var dummy = new DummyClass
            {
                AnotherProperty = "Other value",
                SampleProperty = "Will be overwritten"
            };

            var loader = new XmlLoader(new DummyParserFactory(RuntimeTypeSource));

            var actual = loader.FromString("<DummyClass xmlns=\"root\" SampleProperty=\"Value\" />", dummy);

            Assert.IsType(dummy.GetType(), actual);
            Assert.Equal("Value", ((DummyClass)actual).SampleProperty);
            Assert.Equal("Other value", ((DummyClass)actual).AnotherProperty);
        }
    }
}