namespace OmniXaml.Testing.Common
{
    using Builder;

    public interface IResourceBuilder
    {
        TestRuntimeTypeSource RuntimeTypeSource { get; }
        NamespaceDeclaration RootNs { get; }
        NamespaceDeclaration AnotherNs { get; }
        NamespaceDeclaration SpecialNs { get; }
        XamlInstructionBuilder X { get; }
        ProtoInstructionBuilder P { get; }
    }
}