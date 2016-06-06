namespace OmniXaml.Testing.Resources
{
    using System.Collections.Generic;
    using System.IO;

    public static class File
    {
        public static string LoadAsString(string relativePath)
        {
            var root = @"..\OmniXaml.Testing.Resources";
            var path = Path.Combine(root, relativePath);

            using (var file = new StreamReader(new FileStream(path, FileMode.Open)))
            {               
                var str = file.ReadToEnd();
                return str;
            }
        }

        public static IEnumerable<string> GetFiles(string folder)
        {
            return Directory.GetFiles(folder);
        }
    }
}