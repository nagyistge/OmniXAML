namespace OmniXaml.Tests.ObjectAssemblerTests
{
    using System.Linq;
    using Testing.Classes;
    using Testing.Classes.WpfLikeModel;
    using Xunit;

    public class InstanceNaming 
    {
        public InstanceNaming()
        {
            Fixture = new ObjectAssemblerFixture();
        }

        protected ObjectAssemblerFixture Fixture { get; set; }

        [Fact]
        public void NamedObject_HasCorrectName()
        {
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.NamedObject);
            var result = sut.Result;
            var tb = (TextBlock)result;

            Assert.Equal("MyTextBlock", tb.Name);
        }

        [Fact]
        public void TwoNestedNamedObjects_HaveCorrectNames()
        {
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.TwoNestedNamedObjects);
            var result = sut.Result;
            var lbi = (ListBoxItem)result;
            var textBlock = (TextBlock)lbi.Content;

            Assert.Equal("MyListBoxItem", lbi.Name);
            Assert.Equal("MyTextBlock", textBlock.Name);
        }

        [Fact]
        public void ListBoxWithItemAndTextBlockWithNames_HaveCorrectNames()
        {
            var sut = Fixture.CreateObjectAssembler();
            sut.Process(Fixture.Resources.ListBoxWithItemAndTextBlockWithNames);

            var w = (Window)sut.Result;
            var lb = (ListBox)w.Content;
            var lvi = (ListBoxItem)lb.Items.First();
            var tb = (TextBlock)lvi.Content;

            Assert.Equal("MyListBox", lb.Name);
            Assert.Equal("MyListBoxItem", lvi.Name);
            Assert.Equal("MyTextBlock", tb.Name);
        }
    }
}