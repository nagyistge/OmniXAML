namespace OmniXaml.Tests.ObjectAssemblerTests
{
    using Testing.Classes;
    using Xunit;

    public class Basic : ObjectAssemblerTests
    {
        [Fact]
        public void ObjectWithChild()
        {
            IObjectAssembler sut = CreateSut();
            sut.Process(Resources.ObjectWithChild);

            var result = sut.Result;
            var property = ((DummyClass)result).Child;

            Assert.IsType(typeof(DummyClass), result);
            Assert.IsType(typeof(ChildClass), property);
        }

        [Fact]
        public void ObjectWithEnumMember()
        {
            IObjectAssembler sut = CreateSut();
            sut.Process(Resources.ObjectWithEnumMember);

            var result = sut.Result;
            var property = ((DummyClass)result).EnumProperty;

            Assert.IsType<DummyClass>(result);
            Assert.Equal(SomeEnum.One, property);
        }

        [Fact]
        public void ObjectWithMember()
        {
            IObjectAssembler sut = CreateSut();
            sut.Process(Resources.ObjectWithMember);

            var result = sut.Result;
            var property = ((DummyClass)result).SampleProperty;

            Assert.IsType(typeof(DummyClass), result);
            Assert.Equal("Property!", property);
        }

        [Fact]
        public void ObjectWithNullableEnumProperty()
        {
            IObjectAssembler sut = CreateSut();
            sut.Process(Resources.ObjectWithNullableEnumProperty);

            var result = sut.Result;
            var property = ((DummyClass)result).EnumProperty;

            Assert.IsType<DummyClass>(result);
            Assert.Equal(SomeEnum.One, property);
        }

        [Fact]
        public void ObjectWithTwoMembers()
        {
            IObjectAssembler sut = CreateSut();
            sut.Process(Resources.ObjectWithTwoMembers);

            var result = sut.Result;
            var property1 = ((DummyClass)result).SampleProperty;
            var property2 = ((DummyClass)result).AnotherProperty;

            Assert.IsType(typeof(DummyClass), result);
            Assert.Equal("Property!", property1);
            Assert.Equal("Another!", property2);
        }

        [Fact]
        public void OneObject()
        {
            IObjectAssembler sut = CreateSut();
            sut.Process(Resources.OneObject);

            var result = sut.Result;

            Assert.IsType(typeof(DummyClass), result);
        }

        [Fact]
        public void String()
        {
            var sysNs = new NamespaceDeclaration("clr-namespace:System;assembly=mscorlib", "sys");

            IObjectAssembler sut = CreateSut();
            sut.Process(Resources.GetString(sysNs));

            var actual = sut.Result;
            Assert.IsType(typeof(string), actual);
            Assert.Equal("Text", actual);
        }
    }
}