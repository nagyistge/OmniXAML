namespace OmniXaml.Tests.ObjectAssemblerTests
{
    using System.Collections.Generic;
    using ObjectAssembler;
    using TypeConversion;

    public class ObjectAssemblerFixture : ObjectAssemblerFixtureBase
    {
        public override IObjectAssembler CreateObjectAssembler()
        {
            var topDownValueContext = new TopDownValueContext();
            var valueConnectionContext = new ValueContext(RuntimeTypeSource, topDownValueContext, new Dictionary<string, object>());
            return new ObjectAssembler(RuntimeTypeSource, valueConnectionContext, new Settings { InstanceLifeCycleListener = new TestListener() });
        }
    }
}