namespace OmniXaml.Testing.Classes
{
    using Attributes;

    public class Selector : ItemsControl
    {
        [DependsOn("Items")]
        public int SelectedIndex { get; set; }
    }
}