namespace OmniXaml.Tests.ObjectAssemblerTests
{
    using System.Collections;
    using Testing.Classes;
    using Xunit;

    public class Extensions : ObjectAssemblerTests
    {
        [Fact]
        public void ExtensionThatReturnsNull()
        {
            IObjectAssembler sut = CreateSut();
            sut.Process(Resources.ExtensionThatReturnsNull);

            var result = sut.Result;
            var property = ((DummyClass)result).SampleProperty;

            Assert.IsType(typeof(DummyClass), result);
            Assert.Null(property);
        }

        [Fact]
        public void ExtensionWithArgument()
        {
            IObjectAssembler sut = CreateSut();
            sut.Process(Resources.ExtensionWithArgument);

            var result = sut.Result;
            var property = ((DummyClass)result).SampleProperty;

            Assert.IsType(typeof(DummyClass), result);
            Assert.Equal("Option", property);
        }

        [Fact]
        public void ExtensionWithNonStringArgument()
        {
            IObjectAssembler sut = CreateSut();
            sut.Process(Resources.ExtensionWithNonStringArgument);

            var result = sut.Result;
            var property = ((DummyClass)result).Number;

            Assert.IsType(typeof(DummyClass), result);
            Assert.Equal(123, property);
        }

        [Fact]
        public void ExtensionWithTwoArguments()
        {
            IObjectAssembler sut = CreateSut();
            sut.Process(Resources.ExtensionWithTwoArguments);

            var result = sut.Result;
            var property = ((DummyClass)result).SampleProperty;

            Assert.IsType(typeof(DummyClass), result);
            Assert.Equal("OneSecond", property);
        }

        [Fact]
        public void CollectionAsRootWithManyElements()
        {
            IObjectAssembler sut = CreateSut();
            sut.Process(Resources.MixedCollection);
            var result = sut.Result;
            Assert.IsType(typeof(ArrayList), result);
            var arrayList = (ArrayList)result;
            Assert.True(arrayList.Count > 0);
        }
    }
}