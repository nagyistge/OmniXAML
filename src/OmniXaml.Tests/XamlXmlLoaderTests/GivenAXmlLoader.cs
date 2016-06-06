namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using Testing.Common;

    public class GivenAXmlLoader : GivenARuntimeTypeSource
    {
        protected GivenAXmlLoader()
        {
            Loader = new XmlLoader(new DummyParserFactory(RuntimeTypeSource));
        }

        protected XmlLoader Loader { get; }
    }
}
