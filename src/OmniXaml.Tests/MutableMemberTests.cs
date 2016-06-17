namespace OmniXaml.Tests
{
    using ObjectAssemblerTests.New;
    using Testing.Classes;
    using Xunit;

    public class MutableMemberTests
    {
        [Fact]
        public void TestNonWritable()
        {
            var fixture = new PureObjectAssemblerFixture();
            var type = fixture.RuntimeTypeSource.GetByType(typeof(ImmutableDummy));
            var property = type.GetMember("Text");
            Assert.False(property.IsWritable);
        }
    }
}