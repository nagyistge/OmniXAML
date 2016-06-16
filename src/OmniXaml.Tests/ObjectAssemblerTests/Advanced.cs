namespace OmniXaml.Tests.ObjectAssemblerTests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Testing.Classes;
    using Testing.Classes.WpfLikeModel;
    using Xunit;

    public class Advanced
    {
        public Advanced()
        {
            Fixture = new ObjectAssemblerFixture();
        }

        public ObjectAssemblerFixtureBase Fixture { get; set; }

        [Fact]
        public void AttemptToAssignItemsToNonCollectionMember()
        {
            Assert.Throws<ParseException>(() => Fixture.CreateObjectAssembler().Process(Fixture.Resources.AttemptToAssignItemsToNonCollectionMember));
        }

        [Fact]
        public void ChildIsAssociatedBeforeItsPropertiesAreSet()
        {
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.InstanceWithChildAndProperty);
            var result = (DummyClass) sut.Result;

            Assert.False(result.TitleWasSetBeforeBeingAssociated);
        }

        [Fact]
        public void CorrectInstanceSetupSequence()
        {
            var expectedSequence = new[] {SetupSequence.Begin, SetupSequence.AfterAssociatedToParent, SetupSequence.AfterSetProperties, SetupSequence.End};
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.InstanceWithChild);

            var listener = (TestListener) sut.LifecycleListener;
            Assert.Equal(expectedSequence.ToList().AsReadOnly(), listener.InvocationOrder);
        }

        [Fact]
        public void DirectContentForOneToMany()
        {
            Fixture.CreateObjectAssembler().Process(Fixture.Resources.DirectContentForOneToMany);
        }

        [Fact]
        public void ExpandedAttachablePropertyAndItemBelow()
        {
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.ExpandedAttachablePropertyAndItemBelow);

            var items = ((DummyClass) sut.Result).Items;

            var firstChild = items.First();
            var attachedProperty = Container.GetProperty(firstChild);
            Assert.Equal(2, items.Count);
            Assert.Equal("Value", attachedProperty);
        }

        [Fact]
        public void KeyDirective()
        {
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.KeyDirective);

            var actual = sut.Result;
            Assert.IsType(typeof(DummyClass), actual);
            var dictionary = (IDictionary) ((DummyClass) actual).Resources;
            Assert.True(dictionary.Count > 0);
        }

        [Fact]
        public void ListBoxWithItemAndTextBlockNoNames()
        {
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.ListBoxWithItemAndTextBlockNoNames);

            var w = (Window) sut.Result;
            var lb = (ListBox) w.Content;
            var lvi = (ListBoxItem) lb.Items.First();
            var tb = lvi.Content;

            Assert.IsType(typeof(TextBlock), tb);
        }

        [Fact]
        public void MemberAfterInitalizationValue()
        {
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.MemberAfterInitalizationValue);

            var root = (RootObject) sut.Result;
            var str = root.Collection[0];
            var dummy = (DummyClass) root.Collection[1];

            Assert.Equal("foo", str);
            Assert.Equal(123, dummy.Number);
        }

        [Fact]
        public void MemberWithIncompatibleTypes()
        {
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.MemberWithIncompatibleTypes);

            var result = sut.Result;
            var property = ((DummyClass) result).Number;

            Assert.IsType(typeof(DummyClass), result);
            Assert.Equal(12, property);
        }

        [Fact]
        public void PropertyShouldBeAssignedBeforeChildIsAssociatedToItsParent()
        {
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.ParentShouldReceiveInitializedChild);
            var parent = (SpyingParent) sut.Result;
            Assert.True(parent.ChildHadNamePriorToBeingAssigned);
        }      

        [Fact]
        public void RootInstanceWithAttachableMember()
        {
            var root = new DummyClass();
            var sut = Fixture.CreateSutForLoadingSpecificInstance(root);
            sut.Process(Fixture.Resources.RootInstanceWithAttachableMember);
            var result = sut.Result;
            var attachedProperty = Container.GetProperty(result);
            Assert.Equal("Value", attachedProperty);
        }

        [Fact]
        public void TopDownContainsOuterObject()
        {
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.InstanceWithChild);

            var dummyClassXamlType = Fixture.RuntimeTypeSource.GetByType(typeof(DummyClass));
            var lastInstance = sut.TopDownValueContext.GetLastInstance(dummyClassXamlType);

            Assert.IsType(typeof(DummyClass), lastInstance);
        }

        [Fact]
        public void TwoChildrenWithNoRoot_ShouldThrow()
        {
            Assert.Throws<ParseException>(() => Fixture.CreateObjectAssembler().Process(Fixture.Resources.TwoRoots));
        }      
    }
}