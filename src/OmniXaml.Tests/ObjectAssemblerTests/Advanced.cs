namespace OmniXaml.Tests.ObjectAssemblerTests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Testing.Classes;
    using Testing.Classes.WpfLikeModel;
    using Xunit;

    public class Advanced : ObjectAssemblerTests
    {
        [Fact]
        public void AttemptToAssignItemsToNonCollectionMember()
        {
            Assert.Throws<ParseException>(() => CreateSut().Process(Resources.AttemptToAssignItemsToNonCollectionMember));
        }

        [Fact]
        public void ChildIsAssociatedBeforeItsPropertiesAreSet()
        {
            IObjectAssembler sut = CreateSut();
            sut.Process(Resources.InstanceWithChildAndProperty);
            var result = (DummyClass) sut.Result;

            Assert.False(result.TitleWasSetBeforeBeingAssociated);
        }

        [Fact]
        public void CorrectInstanceSetupSequence()
        {
            var expectedSequence = new[] {SetupSequence.Begin, SetupSequence.AfterAssociatedToParent, SetupSequence.AfterSetProperties, SetupSequence.End};
            IObjectAssembler sut = CreateSut();
            sut.Process(Resources.InstanceWithChild);

            var listener = (TestListener) sut.LifecycleListener;
            Assert.Equal(expectedSequence.ToList().AsReadOnly(), listener.InvocationOrder);
        }

        [Fact]
        public void DirectContentForOneToMany()
        {
            CreateSut().Process(Resources.DirectContentForOneToMany);
        }

        [Fact]
        public void ExpandedAttachablePropertyAndItemBelow()
        {
            IObjectAssembler sut = CreateSut();
            sut.Process(Resources.ExpandedAttachablePropertyAndItemBelow);

            var items = ((DummyClass) sut.Result).Items;

            var firstChild = items.First();
            var attachedProperty = Container.GetProperty(firstChild);
            Assert.Equal(2, items.Count);
            Assert.Equal("Value", attachedProperty);
        }

        [Fact]
        public void KeyDirective()
        {
            IObjectAssembler sut = CreateSut();
            sut.Process(Resources.KeyDirective);

            var actual = sut.Result;
            Assert.IsType(typeof(DummyClass), actual);
            var dictionary = (IDictionary) ((DummyClass) actual).Resources;
            Assert.True(dictionary.Count > 0);
        }

        [Fact]
        public void ListBoxWithItemAndTextBlockNoNames()
        {
            IObjectAssembler sut = CreateSut();
            sut.Process(Resources.ListBoxWithItemAndTextBlockNoNames);

            var w = (Window) sut.Result;
            var lb = (ListBox) w.Content;
            var lvi = (ListBoxItem) lb.Items.First();
            var tb = lvi.Content;

            Assert.IsType(typeof(TextBlock), tb);
        }

        [Fact]
        public void MemberAfterInitalizationValue()
        {
            IObjectAssembler sut = CreateSut();
            sut.Process(Resources.MemberAfterInitalizationValue);

            var root = (RootObject) sut.Result;
            var str = root.Collection[0];
            var dummy = (DummyClass) root.Collection[1];

            Assert.Equal("foo", str);
            Assert.Equal(123, dummy.Number);
        }

        [Fact]
        public void MemberWithIncompatibleTypes()
        {
            IObjectAssembler sut = CreateSut();
            sut.Process(Resources.MemberWithIncompatibleTypes);

            var result = sut.Result;
            var property = ((DummyClass) result).Number;

            Assert.IsType(typeof(DummyClass), result);
            Assert.Equal(12, property);
        }

        [Fact]
        public void PropertyShouldBeAssignedBeforeChildIsAssociatedToItsParent()
        {
            IObjectAssembler sut = CreateSut();
            sut.Process(Resources.ParentShouldReceiveInitializedChild);
            var parent = (SpyingParent) sut.Result;
            Assert.True(parent.ChildHadNamePriorToBeingAssigned);
        }      

        [Fact]
        public void RootInstanceWithAttachableMember()
        {
            var root = new DummyClass();
            var sut = CreateSutForLoadingSpecificInstance(root);
            sut.Process(Resources.RootInstanceWithAttachableMember);
            var result = sut.Result;
            var attachedProperty = Container.GetProperty(result);
            Assert.Equal("Value", attachedProperty);
        }

        [Fact]
        public void TopDownContainsOuterObject()
        {
            IObjectAssembler sut = CreateSut();
            sut.Process(Resources.InstanceWithChild);

            var dummyClassXamlType = RuntimeTypeSource.GetByType(typeof(DummyClass));
            var lastInstance = sut.TopDownValueContext.GetLastInstance(dummyClassXamlType);

            Assert.IsType(typeof(DummyClass), lastInstance);
        }

        [Fact]
        public void TwoChildrenWithNoRoot_ShouldThrow()
        {
            Assert.Throws<ParseException>(() => CreateSut().Process(Resources.TwoRoots));
        }      
    }
}