namespace Xaml.Tests.Resources
{
    using System.Collections.Generic;
    using System.IO;

    public static class File
    {
        public static string LoadAsString(string path)
        {
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