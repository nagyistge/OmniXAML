namespace OmniXaml.Tests.ObjectAssemblerTests
{
    using System.Collections.Generic;
    using Builder;
    using ObjectAssembler;
    using Resources;
    using Testing.Common;
    using TypeConversion;
    using Typing;

    public class ObjectAssemblerFixture : IResourceBuilder
    {
        public ObjectAssemblerFixture()
        {
            RuntimeTypeSource = new TestRuntimeTypeSource();
            X = new XamlInstructionBuilder(RuntimeTypeSource);
            P = new ProtoInstructionBuilder(RuntimeTypeSource);
            Resources = new InstructionResources(this);
        }

        public InstructionResources Resources { get; set; }

        public NamespaceDeclaration RootNs { get; } = new NamespaceDeclaration("root", string.Empty);
        public NamespaceDeclaration AnotherNs { get; } = new NamespaceDeclaration("another", "a");
        public NamespaceDeclaration SpecialNs { get; } = new NamespaceDeclaration(CoreTypes.SpecialNamespace, "x");

        public ProtoInstructionBuilder P { get; set; }

        public XamlInstructionBuilder X { get; set; }

        public TestRuntimeTypeSource RuntimeTypeSource { get; set; }

        public IObjectAssembler CreateObjectAssembler()
        {
            var topDownValueContext = new TopDownValueContext();
            var valueConnectionContext = new ValueContext(RuntimeTypeSource, topDownValueContext, new Dictionary<string, object>());
            return new ObjectAssembler(RuntimeTypeSource, valueConnectionContext, new Settings { InstanceLifeCycleListener = new TestListener() });
        }

        public IObjectAssembler CreateSutForLoadingSpecificInstance(object instance)
        {
            var topDownValueContext = new TopDownValueContext();
            var valueConnectionContext = new ValueContext(RuntimeTypeSource, topDownValueContext, new Dictionary<string, object>());

            var settings = new Settings { RootInstance = instance };

            var assembler = new ObjectAssembler(RuntimeTypeSource, valueConnectionContext, settings);
            return assembler;
        }
    }
}