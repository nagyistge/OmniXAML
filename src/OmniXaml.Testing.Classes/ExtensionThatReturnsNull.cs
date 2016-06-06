namespace OmniXaml.Testing.Classes
{
    public class ExtensionThatReturnsNull : IMarkupExtension
    {
        public object ProvideValue(MarkupExtensionContext markupExtensionContext)
        {
            return null;
        }
    }
}