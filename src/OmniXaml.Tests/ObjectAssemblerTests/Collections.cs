﻿namespace OmniXaml.Tests.ObjectAssemblerTests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Testing.Classes;
    using Xunit;

    public class Collections
    {
        public Collections()
        {
            Fixture = new ObjectAssemblerFixture();
        }

        public ObjectAssemblerFixtureBase Fixture { get; set; }

        [Fact]
        public void WithCollection()
        {
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.CollectionWithMoreThanOneItem);

            var result = sut.Result;
            var children = ((DummyClass)result).Items;

            Assert.IsType(typeof(DummyClass), result);
            Assert.Equal(3, children.Count);
            Assert.All(children, child => Assert.IsType(typeof(Item), child));
        }

        [Fact]
        public void MixedCollectionWithRootInstance()
        {
            var root = new List<object>();
            var assembler = Fixture.CreateSutForLoadingSpecificInstance(root);
            assembler.Process(Fixture.Resources.MixedCollection);
            var result = assembler.Result;
            Assert.IsType(typeof(List<object>), result);
            var arrayList = (List<object>)result;
            Assert.True(arrayList.Count > 0);
        }



        [Fact]
        public void WithCollectionAndInnerAttribute()
        {
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.WithCollectionAndInnerAttribute);

            var result = sut.Result;
            var children = ((DummyClass)result).Items;
            var firstChild = children.First();
            var property = firstChild.Title;

            Assert.IsType(typeof(DummyClass), result);
            Assert.All(children, child => Assert.IsType(typeof(Item), child));
            Assert.Equal("SomeText", property);
        }

        [Fact]
        public void ExplicitCollection_ShouldReplaceCollectionInstance()
        {
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.ExplicitCollection);
            var actual = (RootObject)sut.Result;

            Assert.True(actual.CollectionWasReplaced);
        }

        [Fact]
        public void ImplicitCollection_ShouldHaveItems()
        {
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.ImplicitCollection);
            var actual = (RootObject)sut.Result;

            var customCollection = actual.Collection;

            Assert.NotEmpty(customCollection);
        }

        [Fact]
        public void ImplicitCollection_ShouldKeepSameCollectionInstance()
        {
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.ImplicitCollection);
            var actual = (RootObject)sut.Result;

            Assert.False(actual.CollectionWasReplaced);
        }

        [Fact]
        public void ExplicitCollection_ShouldHaveItems()
        {
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.ExplicitCollection);
            var actual = (RootObject)sut.Result;

            var customCollection = actual.Collection;

            Assert.NotEmpty(customCollection);
        }

        [Fact]
        public void CustomCollection()
        {
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.CustomCollection);
            Assert.NotEmpty((IEnumerable)sut.Result);
        }

        [Fact]
        public void CollectionWithInnerCollection()
        {
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.CollectionWithInnerCollection);

            var result = sut.Result;
            var children = ((DummyClass)result).Items;

            Assert.IsType(typeof(DummyClass), result);
            Assert.Equal(3, children.Count);
            Assert.All(children, child => Assert.IsType(typeof(Item), child));
            var innerCollection = children[0].Children;
            Assert.Equal(2, innerCollection.Count);
            Assert.All(innerCollection, child => Assert.IsType(typeof(Item), child));
        }

        [Fact]
        public void AttachableMemberThatIsCollection()
        {
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.AttachableMemberThatIsCollection);
            var instance = sut.Result;
            var col = Container.GetCollection(instance);

            Assert.NotEmpty(col);
        }
    }
}