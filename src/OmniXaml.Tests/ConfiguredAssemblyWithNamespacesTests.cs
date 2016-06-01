namespace OmniXaml.Tests
{
    using System.Reflection;
    using Builder;
    using Xunit;

    public class ConfiguredAssemblyWithNamespacesTests
    {
        [Fact]
        public void LooksInCorrectNamespace()
        {
            var expectedType = typeof(OmniXaml.Testing.Classes.Another.DummyChild);
            var can = new ConfiguredAssemblyWithNamespaces(
                expectedType.GetTypeInfo().Assembly,
                new[] { "OmniXaml.Testing.Classes.Another" });

            var result = can.Get("DummyChild");

            Assert.Equal(expectedType, result);
        }
    }
}
