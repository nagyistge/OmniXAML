namespace OmniXaml.Tests.ObjectAssemblerTests
{
    using System.Collections.Generic;
    using ObjectAssembler;
    using Resources;
    using Testing.Common;
    using TypeConversion;

    public class ObjectAssemblerTests : GivenARuntimeTypeSource
    {
        public ObjectAssemblerTests()
        {
            resources = new InstructionResources(this);
        }

        private readonly InstructionResources resources;

        protected InstructionResources Resources => resources;

        protected IObjectAssembler CreateSut()
        {
            var topDownValueContext = new TopDownValueContext();
            var valueConnectionContext = new ValueContext(RuntimeTypeSource, topDownValueContext, new Dictionary<string, object>());
            return new ObjectAssembler(RuntimeTypeSource, valueConnectionContext, new Settings {InstanceLifeCycleListener = new TestListener()});
        }

        protected IObjectAssembler CreateSutForLoadingSpecificInstance(object instance)
        {
            var topDownValueContext = new TopDownValueContext();
            var valueConnectionContext = new ValueContext(RuntimeTypeSource, topDownValueContext, new Dictionary<string, object>());

            var settings = new Settings {RootInstance = instance};

            var assembler = new ObjectAssembler(RuntimeTypeSource, valueConnectionContext, settings);
            return assembler;
        }
    }
}