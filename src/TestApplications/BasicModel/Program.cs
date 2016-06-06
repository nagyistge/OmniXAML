namespace BasicModel
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Model;
    using OmniXaml;
    using OmniXaml.ObjectAssembler;
    using OmniXaml.Services;

    internal static class Program
    {
        private static void Main()
        {
            var runtimeTypeSource = RuntimeTypeSource.FromAttributes(new [] { Assembly.GetEntryAssembly()});
            
            var loader = new DefaultLoader(runtimeTypeSource);

            var dict = new Dictionary<string, object> {{"Hola", "Tío"}};
            var model = (Zoo)loader.FromPath("Model.xaml", new Settings { InstanceLifeCycleListener = new DefaultInstanceLifeCycleListener(), ParsingContext = dict });
            var byName = model.Find("Rocky");

            Console.WriteLine("Loaded model:\n{0}", model);
            Console.WriteLine($"Searching an animal by name in this namescope (Zoo instance): \n\tRocky => {byName}");
            Console.ReadLine();
        }
    }
}
