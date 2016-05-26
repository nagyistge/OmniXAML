namespace OmniXaml.Testing.Classes
{
    using Attributes;

    [ContentProperty("Content")]
    public class ContentControl : DummyObject
    {
        public object Content { get; set; }        
    }
}