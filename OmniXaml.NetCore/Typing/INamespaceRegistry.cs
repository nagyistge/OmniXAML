namespace OmniXaml.Typing
{
    using System.Collections.Generic;
    using Builder;

    public interface INamespaceRegistry
    {
        Namespace GetNamespace(string name);
        Namespace GetNamespaceByPrefix(string prefix);
        void RegisterPrefix(PrefixRegistration prefixRegistration);
        void AddNamespace(XamlNamespace xamlNamespace);
        IEnumerable<PrefixRegistration> RegisteredPrefixes { get; }
    }
}