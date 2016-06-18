namespace OmniXaml.Testing.Classes
{
    public class ImmutableDummy
    {
        public ImmutableDummy(string text)
        {
            Text = text;
        }

        public ImmutableDummy(string text, ChildClass child) : this(text)
        {
            Text = text;
            Child = child;
        }

        public string Text { get; }

        public ChildClass Child { get; }
    }
}