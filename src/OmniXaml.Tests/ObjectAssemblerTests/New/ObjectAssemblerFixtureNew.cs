namespace OmniXaml.Tests.ObjectAssemblerTests.New
{
    using System.Collections.Generic;
    using ObjectAssembler;
    using TypeConversion;

    public class ObjectAssemblerFixtureNew : ObjectAssemblerFixtureBase
    {
        public override IObjectAssembler CreateObjectAssembler()
        {
            var topDownValueContext = new TopDownValueContext();
            var valueConnectionContext = new ValueContext(RuntimeTypeSource, topDownValueContext, new Dictionary<string, object>());
            return new PureObjectAssembler(valueConnectionContext);
        }
    }
}