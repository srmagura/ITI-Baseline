using System;
using Newtonsoft.Json;

namespace Iti.Utilities
{
    public static class JsonExtensions
    {
        public static void ConsoleDump(this object obj, string tag = null)
        {
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);

            tag = tag == null ? "" : $" {tag} ";
            Console.WriteLine($"==={tag}===================================================");
            Console.WriteLine(json);
        }
    }
}