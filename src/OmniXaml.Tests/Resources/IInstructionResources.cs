namespace OmniXaml.Tests.Resources
{
    using System.Collections.Generic;

    public interface IInstructionResources
    {
        NamespaceDeclaration SpecialNs { get; set; }
        IEnumerable<Instruction> ExtensionWithTwoArguments { get; }
        IEnumerable<Instruction> ExtensionWithNonStringArgument { get; }
        IEnumerable<Instruction> OneObject { get; }
        IEnumerable<Instruction> ObjectWithMember { get; }
        IEnumerable<Instruction> ObjectWithEnumMember { get; }
        IEnumerable<Instruction> ObjectWithNullableEnumProperty { get; }
        IEnumerable<Instruction> ObjectWithTwoMembers { get; }
        IEnumerable<Instruction> CollectionWithInnerCollection { get; }
        IEnumerable<Instruction> WithCollectionAndInnerAttribute { get; }
        IEnumerable<Instruction> MemberWithIncompatibleTypes { get; }
        IEnumerable<Instruction> ExtensionWithArgument { get; }
        IEnumerable<Instruction> ExtensionThatReturnsNull { get; }
        IEnumerable<Instruction> NestedChild { get; }
        IEnumerable<Instruction> InstanceWithChild { get; }
        IEnumerable<Instruction> InstanceWithChildAndProperty { get; }
        IEnumerable<Instruction> ObjectWithChild { get; }
        IEnumerable<Instruction> ComplexNesting { get; }
        IEnumerable<Instruction> CollectionWithMoreThanOneItem { get; }
        IEnumerable<Instruction> ImmutableCollectionWithMoreThanOneItem { get; }
        IEnumerable<Instruction> NestedChildWithContentProperty { get; }
        IEnumerable<Instruction> TwoNestedPropertiesUsingContentProperty { get; }
        IEnumerable<Instruction> MixedPropertiesWithContentPropertyAfter { get; }
        IEnumerable<Instruction> CollectionWithMixedEmptyAndNotEmptyNestedElements { get; }
        IEnumerable<Instruction> MixedPropertiesWithContentPropertyBefore { get; }
        IEnumerable<Instruction> TwoNestedPropertiesOneOfThemUsesContentPropertyWithSingleItem { get; }
        IEnumerable<Instruction> TwoNestedProperties { get; }
        IEnumerable<Instruction> ContentPropertyNesting { get; }
        IEnumerable<Instruction> ElementWithTwoDeclarations { get; }
        IEnumerable<Instruction> KeyDirective { get; }
        IEnumerable<Instruction> KeyDirective2 { get; }
        IEnumerable<Instruction> DifferentNamespacesAndMoreThanOneProperty { get; }
        IEnumerable<Instruction> ContentPropertyForSingleProperty { get; }
        IEnumerable<Instruction> ContentPropertyForCollectionMoreThanOneElement { get; }
        IEnumerable<Instruction> CollapsedTagWithProperty { get; }
        IEnumerable<Instruction> SingleInstance { get; }
        IEnumerable<Instruction> DifferentNamespaces { get; }
        IEnumerable<Instruction> CollectionWithOneItem { get; }
        IEnumerable<Instruction> CollectionWithOneItemAndAMember { get; }
        IEnumerable<Instruction> ExpandedStringProperty { get; }
        IEnumerable<Instruction> TestReverseMembersReverted { get; }
        IEnumerable<Instruction> TestReverseMembers { get; }
        IEnumerable<Instruction> TwoMembersReversed { get; }
        IEnumerable<Instruction> TwoMembers { get; }
        List<Instruction> SimpleExtensionWithOneAssignment { get; }
        List<Instruction> SimpleExtension { get; }
        List<Instruction> ContentPropertyForCollectionOneElement { get; }
        IEnumerable<Instruction> TextBlockWithText { get; }
        IEnumerable<Instruction> ChildInNameScope { get; }
        IEnumerable<Instruction> ChildInDeeperNameScope { get; }
        IEnumerable<Instruction> NameWithNoNamescopesToRegisterTo { get; }
        List<Instruction> ComboBoxCollectionOnly { get; }
        List<Instruction> StyleSorted { get; }
        List<Instruction> StyleUnsorted { get; }
        List<Instruction> SetterUnsorted { get; }
        List<Instruction> SetterSorted { get; }
        List<Instruction> ComboBoxUnsorted { get; }
        List<Instruction> ComboBoxSorted { get; }
        List<Instruction> TwoComboBoxesUnsorted { get; }
        List<Instruction> TwoComboBoxesSorted { get; }
        List<Instruction> ListBoxSortedWithExtension { get; }
        IEnumerable<Instruction> AttemptToAssignItemsToNonCollectionMember { get; }
        IEnumerable<Instruction> TwoRoots { get; }
        IEnumerable<Instruction> ParentShouldReceiveInitializedChild { get; }
        IEnumerable<Instruction> MixedCollection { get; }
        IEnumerable<Instruction> ListBoxWithItemAndTextBlockWithNames { get; }
        IEnumerable<Instruction> ListBoxWithItemAndTextBlockNoNames { get; }
        IEnumerable<Instruction> NamedObject { get; }
        IEnumerable<Instruction> TwoNestedNamedObjects { get; }
        IEnumerable<Instruction> RootInstanceWithAttachableMember { get; }
        IEnumerable<Instruction> DirectContentForOneToMany { get; }
        IEnumerable<Instruction> ExpandedAttachablePropertyAndItemBelow { get; }
        IEnumerable<Instruction> CustomCollection { get; }
        IEnumerable<Instruction> AttachableMemberThatIsCollection { get; }
        IEnumerable<Instruction> AttachableMemberThatIsCollectionImplicit { get; }
        IEnumerable<Instruction> PureCollection { get; }
        IEnumerable<Instruction> ExplicitCollection { get; }
        IEnumerable<Instruction> ImplicitCollection { get; }
        IEnumerable<Instruction> MemberAfterInitalizationValue { get; }
        IEnumerable<Instruction> OneImmutableObject { get; }
        IEnumerable<Instruction> StringInitialization(NamespaceDeclaration sysNs);
        IEnumerable<Instruction> CreateExpectedNodesForImplicitContentPropertyWithImplicityCollection();
    }
}