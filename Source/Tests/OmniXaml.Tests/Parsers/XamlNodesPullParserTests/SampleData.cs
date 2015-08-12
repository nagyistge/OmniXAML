namespace OmniXaml.Tests.Parsers.XamlNodesPullParserTests
{
    using System.Collections.Generic;
    using Builder;
    using Classes;

    internal class SampleData
    {
        private readonly ProtoInstructionBuilder p;
        private readonly XamlInstructionBuilder x;

        public SampleData(ProtoInstructionBuilder p, XamlInstructionBuilder x)
        {
            this.p = p;
            this.x = x;
        }

        public ProtoInstructionBuilder P
        {
            get { return p; }
        }

        public XamlInstructionBuilder X
        {
            get { return x; }
        }

        public List<XamlInstruction> CreateExpectedNodesForTwoNestedPropertiesOneOfThemUsesContentPropertyWithSingleItem(NamespaceDeclaration rootNs)
        {
            var expectedInstructions = new List<XamlInstruction>
            {
                X.NamespacePrefixDeclaration(rootNs), X.StartObject(typeof (ChildClass)), X.StartMember<ChildClass>(d => d.Content), X.StartObject(typeof (Item)), X.StartMember<Item>(item => item.Children), X.GetObject(), X.Items(), X.StartObject(typeof (Item)), X.StartMember<Item>(i => i.Title), X.Value("Item1"), X.EndMember(), X.EndObject(), X.EndMember(), X.EndObject(), X.EndMember(), X.EndObject(), X.EndMember(), X.StartMember<DummyClass>(d => d.Child), X.StartObject(typeof (ChildClass)), X.EndObject(), X.EndMember(), X.EndObject(),
            };
            return expectedInstructions;
        }

        public IEnumerable<ProtoXamlInstruction> CreateInputForTwoNestedPropertiesOneOfThemUsesContentPropertyWithSingleItem(NamespaceDeclaration rootNs)
        {
            var input = new List<ProtoXamlInstruction>
            {
                P.NamespacePrefixDeclaration(rootNs),
                P.NonEmptyElement(typeof (ChildClass), rootNs),
                P.NonEmptyElement(typeof (Item), rootNs),
                P.EmptyElement(typeof (Item), rootNs),
                P.Attribute<Item>(d => d.Title, "Item1", rootNs),
                P.Text(),
                P.EndTag(),
                P.Text(),
                P.NonEmptyPropertyElement<DummyClass>(d => d.Child, rootNs),
                P.EmptyElement(typeof (ChildClass), rootNs),
                P.Text(),
                P.EndTag(),
                P.EndTag(),
            };
            return input;
        }

        public List<XamlInstruction> CreateExpectedNodesForTwoNestedProperties(NamespaceDeclaration rootNs)
        {
            var expectedInstructions = new List<XamlInstruction>
            {
                X.NamespacePrefixDeclaration(rootNs),
                X.StartObject(typeof (DummyClass)),
                X.StartMember<DummyClass>(d => d.Items),
                X.GetObject(),
                X.Items(),
                X.StartObject(typeof (Item)),
                X.StartMember<Item>(i => i.Title),
                X.Value("Main1"),
                X.EndMember(),
                X.EndObject(),
                X.StartObject(typeof (Item)),
                X.StartMember<Item>(i => i.Title),
                X.Value("Main2"),
                X.EndMember(),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
                X.EndMember(),
                X.StartMember<DummyClass>(d => d.Child),
                X.StartObject(typeof (ChildClass)),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
            };
            return expectedInstructions;
        }

        public List<XamlInstruction> CreateExpectedNodesCollectionsContentPropertyNesting(NamespaceDeclaration rootNs)
        {
            return new List<XamlInstruction>
            {
                X.NamespacePrefixDeclaration(rootNs),
                X.StartObject(typeof (DummyClass)),
                X.StartMember<DummyClass>(d => d.Items),
                X.GetObject(),
                X.Items(),
                X.StartObject(typeof (Item)),
                X.StartMember<Item>(i => i.Title),
                X.Value("Main1"),
                X.EndMember(),
                X.EndObject(),
                X.StartObject(typeof (Item)),
                X.StartMember<Item>(i => i.Title),
                X.Value("Main2"),
                X.EndMember(),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
                X.EndMember(),
                X.StartMember<DummyClass>(d => d.Child),
                X.StartObject(typeof (ChildClass)),
                X.StartMember<ChildClass>(d => d.Content),
                X.StartObject(typeof (Item)),
                // Collection of Items
                X.StartMember<Item>(i => i.Children),
                X.GetObject(),
                X.Items(),
                X.StartObject(typeof (Item)),
                X.StartMember<Item>(i => i.Title),
                X.Value("Item1"),
                X.EndMember(),
                X.EndObject(),
                X.StartObject(typeof (Item)),
                X.StartMember<Item>(i => i.Title),
                X.Value("Item2"),
                X.EndMember(),
                X.EndObject(),
                X.StartObject(typeof (Item)),
                X.StartMember<Item>(i => i.Title),
                X.Value("Item3"),
                X.EndMember(),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
                X.EndMember(),
                // End of collection of items

                X.EndObject(),
                X.EndMember(),
                X.StartMember<ChildClass>(c => c.Child),
                X.StartObject(typeof (ChildClass)),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
            };
        }

        public IEnumerable<ProtoXamlInstruction> CreateInputForCollectionsContentPropertyNesting(NamespaceDeclaration rootNs)
        {
            
            return new List<ProtoXamlInstruction>
            {
                P.NamespacePrefixDeclaration(rootNs),
                P.NonEmptyElement(typeof (DummyClass), rootNs),
                P.EmptyElement(typeof (Item), rootNs),
                P.Attribute<Item>(d => d.Title, "Main1", rootNs),
                P.Text(),
                P.EmptyElement(typeof (Item), rootNs),
                P.Attribute<Item>(d => d.Title, "Main2", rootNs),
                P.Text(),
                P.NonEmptyPropertyElement<DummyClass>(d => d.Child, rootNs),
                P.NonEmptyElement(typeof (ChildClass), rootNs),
                P.NonEmptyElement(typeof (Item), rootNs),
                P.EmptyElement(typeof (Item), rootNs),
                P.Attribute<Item>(d => d.Title, "Item1", rootNs),
                P.Text(),
                P.EmptyElement(typeof (Item), rootNs),
                P.Attribute<Item>(d => d.Title, "Item2", rootNs),
                P.Text(),
                P.EmptyElement(typeof (Item), rootNs),
                P.Attribute<Item>(d => d.Title, "Item3", rootNs),
                P.Text(),
                P.EndTag(),
                P.Text(),
                P.NonEmptyPropertyElement<ChildClass>(c => c.Child, rootNs),
                P.EmptyElement<ChildClass>(rootNs),
                P.Text(),
                P.EndTag(),
                P.EndTag(),
                P.Text(),
                P.EndTag(),
                P.EndTag(),
            };
        }

        public IEnumerable<ProtoXamlInstruction> CreateInputForTwoNestedProperties(NamespaceDeclaration rootNs)
        {
            var input = new List<ProtoXamlInstruction>
            {
                P.NamespacePrefixDeclaration(rootNs),
                P.NonEmptyElement(typeof (DummyClass), rootNs),
                P.NonEmptyPropertyElement<DummyClass>(d => d.Items, rootNs),
                P.EmptyElement(typeof (Item), rootNs),
                P.Attribute<Item>(d => d.Title, "Main1", rootNs),
                P.Text(),
                P.EmptyElement(typeof (Item), rootNs),
                P.Attribute<Item>(d => d.Title, "Main2", rootNs),
                P.Text(),
                P.EndTag(),
                P.NonEmptyPropertyElement<DummyClass>(d => d.Child, rootNs),
                P.NonEmptyElement(typeof (ChildClass), rootNs),
                P.EndTag(),
                P.Text(),
                P.EndTag(),
                P.EndTag(),
            };
            return input;
        }

        public List<XamlInstruction> CreateExpectedNodesForImplicitContentPropertyWithImplicityCollection(NamespaceDeclaration rootNs)
        {
            var expectedInstructions = new List<XamlInstruction>
            {
                X.NamespacePrefixDeclaration(rootNs),
                X.StartObject(typeof (ChildClass)),
                X.StartMember<ChildClass>(d => d.Content),
                X.StartObject(typeof (Item)),
                X.StartMember<Item>(item => item.Children),
                X.GetObject(),
                X.Items(),
                X.StartObject(typeof (Item)),
                X.StartMember<Item>(i => i.Title),
                X.Value("Item1"),
                X.EndMember(),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
            };
            return expectedInstructions;
        }

        public IEnumerable<ProtoXamlInstruction> CreateInputForImplicitContentPropertyWithImplicityCollection(NamespaceDeclaration rootNs)
        {
            
            var input = new List<ProtoXamlInstruction>
            {
                P.NamespacePrefixDeclaration(rootNs),
                P.NonEmptyElement(typeof (ChildClass), rootNs),
                P.NonEmptyElement(typeof (Item), rootNs),
                P.EmptyElement(typeof (Item), rootNs),
                P.Attribute<Item>(d => d.Title, "Item1", rootNs),
                P.Text(),
                P.EndTag(),
                P.EndTag(),
            };

            return input;
        }
    }
}