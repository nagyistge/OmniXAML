namespace OmniXaml.Tests
{
    using System.Collections.Immutable;
    using System.Collections.ObjectModel;
    using Xunit;

    public class ImmutabilityTests
    {
        [Fact]
        public void CollectionCanBeConvertedToImmutable()
        {
            var original = new Collection<object>() { 1, 2, 3, 4, };
            var result = original.AsImmutable(typeof(int));

            Assert.NotEmpty(result);
            Assert.IsAssignableFrom<ImmutableList<int>>(result);
        }
    }   
}