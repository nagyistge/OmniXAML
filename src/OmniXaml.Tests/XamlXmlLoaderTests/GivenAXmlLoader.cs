namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using OmniXaml.Testing.Common;

    public class GivenAXmlLoader : GivenARuntimeTypeSource
    {
        protected GivenAXmlLoader()
        {
            Loader = new XmlLoader(new DummyParserFactory(RuntimeTypeSource));
        }

        protected XmlLoader Loader { get; }
    }
}
