namespace OmniXaml.Tests.ObjectAssemblerTests.New
{
    using Testing.Classes;
    using Xunit;

    public class BasicNew
    {
        public BasicNew()
        {
            Fixture = new ObjectAssemblerFixtureNew();
        }

        public ObjectAssemblerFixtureBase Fixture { get; set; }

        [Fact]
        public void ObjectWithChild()
        {
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.ObjectWithChild);

            var result = sut.Result;
            var property = ((DummyClass)result).Child;

            Assert.IsType(typeof(DummyClass), result);
            Assert.IsType(typeof(ChildClass), property);
        }

        [Fact(Skip = "No")]
        public void ObjectWithEnumMember()
        {
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.ObjectWithEnumMember);

            var result = sut.Result;
            var property = ((DummyClass)result).EnumProperty;

            Assert.IsType<DummyClass>(result);
            Assert.Equal(SomeEnum.One, property);
        }

        [Fact]
        public void ObjectWithStringMember()
        {
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.ObjectWithMember);

            var result = sut.Result;
            var property = ((DummyClass)result).SampleProperty;

            Assert.IsType(typeof(DummyClass), result);
            Assert.Equal("Property!", property);
        }

        [Fact(Skip = "No")]
        public void ObjectWithNullableEnumProperty()
        {
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.ObjectWithNullableEnumProperty);

            var result = sut.Result;
            var property = ((DummyClass)result).EnumProperty;

            Assert.IsType<DummyClass>(result);
            Assert.Equal(SomeEnum.One, property);
        }

        [Fact]
        public void ObjectWithTwoMembers()
        {
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.ObjectWithTwoMembers);

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
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.OneObject);

            var result = sut.Result;

            Assert.IsType(typeof(DummyClass), result);
        }

        [Fact]
        public void Collection()
        {
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.CollectionWithMoreThanOneItemNewAge);

            var result = sut.Result;

            Assert.IsType(typeof(DummyClass), result);
        }

        [Fact(Skip = "No")]
        public void String()
        {
            var sysNs = new NamespaceDeclaration("clr-namespace:System;assembly=mscorlib", "sys");

            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.GetString(sysNs));

            var actual = sut.Result;
            Assert.IsType(typeof(string), actual);
            Assert.Equal("Text", actual);
        }
    }
}