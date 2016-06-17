namespace OmniXaml.Tests.ObjectAssemblerTests.New
{
    using Testing.Classes;
    using Xunit;

    public class BasicNew
    {
        public BasicNew()
        {
            Fixture = new PureObjectAssemblerFixture();
        }

        public PureObjectAssemblerFixture Fixture { get; set; }

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

        [Fact]
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
        public void ObjectWithMember()
        {
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.ObjectWithMember);

            var result = sut.Result;
            var property = ((DummyClass)result).SampleProperty;

            Assert.IsType(typeof(DummyClass), result);
            Assert.Equal("Property!", property);
        }

        [Fact]
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
        public void OneImmutableObject()
        {
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.OneImmutableObject);

            var result = sut.Result;

            Assert.IsType(typeof(ImmutableDummy), result);
            Assert.Equal(((ImmutableDummy)result).Text, "Greetings");
        }
    }
}