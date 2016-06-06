namespace OmniXaml.Services
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public static class Assemblies
    {
        public static IEnumerable<Assembly> ReferencedAssemblies => Assembly.GetEntryAssembly().GetReferencedAssemblies().Select(Assembly.Load);
        public static IEnumerable<Assembly> AssembliesInAppFolder
        {
            get
            {
                var entryAssembly = Assembly.GetEntryAssembly();
                var path = entryAssembly.Location;
                var folder = Path.GetDirectoryName(path);
                var assemblies = new Collection<Assembly>();

                var fileNames = FilterFiles(folder, ".dll", ".exe");

                foreach (var fileName in fileNames)
                {
                    try
                    {
                        assemblies.Add(Assembly.Load(new AssemblyName(fileName)));
                    }
                    catch (FileNotFoundException)
                    {                        
                    }
                    catch (FileLoadException)
                    {
                    }
                    catch (BadImageFormatException)
                    {                        
                    }
                }

                return assemblies;
            }
        }

        public static IEnumerable<string> FilterFiles(string path, params string[] extensionsWithNoWildcard)
        {
            return
                Directory
                .EnumerateFiles(path, "*.*")
                .Where(file => extensionsWithNoWildcard.Any(x => file.EndsWith(x, StringComparison.OrdinalIgnoreCase)));
        }
    }
}