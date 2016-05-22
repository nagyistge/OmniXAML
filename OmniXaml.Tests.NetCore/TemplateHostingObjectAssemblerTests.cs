﻿namespace OmniXaml.Tests
{
    using System.Collections.ObjectModel;
    using Classes;
    using Classes.Templates;
    using Classes.WpfLikeModel;
    using Xunit;
    using ObjectAssembler;
    using TypeConversion;
    using System.Collections.Generic;
    using Common;

    public class TemplateHostingObjectAssemblerTests : GivenARuntimeTypeSource
    {
        [Fact]
        public void SimpleTest()
        {
            var input = new Collection<Instruction>
            {
                X.StartObject<Item>(),
                X.StartMember<Item>(i => i.Template),
                X.StartObject<Template>(),
                X.StartMember<Template>(i => i.Content),
                X.StartObject<Grid>(),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
                X.EndMember(),
                X.EndObject(),
            };

            var mapping = new DeferredLoaderMapping();
            var assembler = new DummyDeferredLoader();
            mapping.Map<Template>(t => t.Content, assembler);

            var objectAssembler = CreateObjectAssembler();

            var sut = new TemplateHostingObjectAssembler(objectAssembler, mapping);                       

            foreach (var instruction in input)
            {
                sut.Process(instruction);
            }

            var actualNodes = sut.NodeList;
            var expectedInstructions = new List<Instruction>
            {
                X.StartObject<Grid>(),
                X.EndObject(),                
            }.AsReadOnly();

            Assert.Equal(expectedInstructions, actualNodes);
            Assert.NotNull(((Item) sut.Result).Template.Content);
        }
    }    
}