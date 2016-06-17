namespace OmniXaml.Tests.ObjectAssemblerTests
{
    using System.Collections;
    using Testing.Classes;
    using Xunit;

    public class Extensions
    {
        public Extensions()
        {
            Fixture = new ObjectAssemblerFixture();
        }

        protected ObjectAssemblerFixture Fixture { get; set; }

        [Fact]
        public void ExtensionThatReturnsNull()
        {
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.ExtensionThatReturnsNull);

            var result = sut.Result;
            var property = ((DummyClass)result).SampleProperty;

            Assert.IsType(typeof(DummyClass), result);
            Assert.Null(property);
        }

        [Fact]
        public void ExtensionWithArgument()
        {
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.ExtensionWithArgument);

            var result = sut.Result;
            var property = ((DummyClass)result).SampleProperty;

            Assert.IsType(typeof(DummyClass), result);
            Assert.Equal("Option", property);
        }

        [Fact]
        public void ExtensionWithNonStringArgument()
        {
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.ExtensionWithNonStringArgument);

            var result = sut.Result;
            var property = ((DummyClass)result).Number;

            Assert.IsType(typeof(DummyClass), result);
            Assert.Equal(123, property);
        }

        [Fact]
        public void ExtensionWithTwoArguments()
        {
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.ExtensionWithTwoArguments);

            var result = sut.Result;
            var property = ((DummyClass)result).SampleProperty;

            Assert.IsType(typeof(DummyClass), result);
            Assert.Equal("OneSecond", property);
        }

        [Fact]
        public void CollectionAsRootWithManyElements()
        {
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.MixedCollection);
            var result = sut.Result;
            Assert.IsType(typeof(ArrayList), result);
            var arrayList = (ArrayList)result;
            Assert.True(arrayList.Count > 0);
        }
    }
}