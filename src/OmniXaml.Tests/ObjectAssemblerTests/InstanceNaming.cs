namespace OmniXaml.Tests.ObjectAssemblerTests
{
    using System.Linq;
    using Testing.Classes;
    using Testing.Classes.WpfLikeModel;
    using Xunit;

    public class InstanceNaming : ObjectAssemblerTests
    {
        [Fact]
        public void NamedObject_HasCorrectName()
        {
            IObjectAssembler sut = CreateSut();
            sut.Process(Resources.NamedObject);
            var result = sut.Result;
            var tb = (TextBlock)result;

            Assert.Equal("MyTextBlock", tb.Name);
        }

        [Fact]
        public void TwoNestedNamedObjects_HaveCorrectNames()
        {
            IObjectAssembler sut = CreateSut();
            sut.Process(Resources.TwoNestedNamedObjects);
            var result = sut.Result;
            var lbi = (ListBoxItem)result;
            var textBlock = (TextBlock)lbi.Content;

            Assert.Equal("MyListBoxItem", lbi.Name);
            Assert.Equal("MyTextBlock", textBlock.Name);
        }

        [Fact]
        public void ListBoxWithItemAndTextBlockWithNames_HaveCorrectNames()
        {
            IObjectAssembler sut = CreateSut();
            sut.Process(Resources.ListBoxWithItemAndTextBlockWithNames);

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