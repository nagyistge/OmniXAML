﻿namespace XamlViewer.ViewModels
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;
    using OmniXaml;
    using OmniXaml.Wpf;
    using XamlViewer;

    public class WpfLoaderViewModel : XamlVisualizerViewModel
    {
        private Snippet selectedSnippet;

        public WpfLoaderViewModel()
        {
            IXamlSnippetProvider snippetsProvider = new SnippetProvider("Xaml\\Wpf");
            Snippets = snippetsProvider.Snippets;
            LoadCommand = new RelayCommand(o => LoadXamlForWpf(), o => Xaml != string.Empty);
            RuntimeTypeSource = new WpfRuntimeTypeSource();
        }


        public IEnumerable<Snippet> Snippets { get; set; }

        public ICommand LoadCommand { get; private set; }

        public Snippet SelectedSnippet
        {
            get { return selectedSnippet; }
            set
            {
                selectedSnippet = value;
                Xaml = string.Copy(SelectedSnippet.Xaml);
                OnPropertyChanged();
            }
        }

        private void LoadXamlForWpf()
        {
            try
            {
                var visualTree = new WpfLoader().FromString(Xaml);

                var window = GetVisualizerWindow(visualTree);
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.DataContext = new TestViewModel();
                window.Show();

            }
            catch (LoadException e)
            {
                ShowProblemLoadingError(e);
            }
        }

        private static Window GetVisualizerWindow(object visualTree)
        {
            var tree = visualTree as Window;
            if (tree != null)
            {
                return tree;
            }

            var window = new Window { Content = visualTree };
            return window;
        }

        private static void ShowProblemLoadingError(LoadException e)
        {
            MessageBox.Show(
                $"There has been a problem loading the XAML at line: {e.LineNumber} pos: {e.LinePosition}. Detailed exception: \n\nException:\n{e.ToString().GetFirstNChars(500)}",
                "Load problem");
        }
    }
}