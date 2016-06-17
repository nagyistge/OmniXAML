namespace OmniXaml.Tests.ObjectAssemblerTests.New
{
    using System.Collections.Generic;
    using ObjectAssembler;
    using Pure;
    using Resources;
    using TypeConversion;

    public class PureObjectAssemblerFixture : ObjectAssemblerFixtureBase
    {
        public override IObjectAssembler CreateObjectAssembler()
        {
            var topDownValueContext = new TopDownValueContext();
            var valueConnectionContext = new ValueContext(RuntimeTypeSource, topDownValueContext, new Dictionary<string, object>());
            return new PureObjectAssembler(valueConnectionContext);
        }

        public PureInstructionResources Resources => new PureInstructionResources(this);
    }
}