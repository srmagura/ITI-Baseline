using System;
using Newtonsoft.Json;

namespace Iti.Utilities
{
    public static class JsonExtensions
    {
        public static void ConsoleDump(this object obj, string tag = null)
        {
            Dump(obj, tag, Console.WriteLine);
        }

        public static void Dump(this object obj, string tag, Action<string> output)
        {
            var json = obj.ToJson();

            tag = tag == null ? "" : $" {tag} ";
            output($"==={tag}===================================================");
            output(json);
        }

        public static string ToJson(this object obj, Formatting formatting = Formatting.Indented)
        {
            return JsonConvert.SerializeObject(obj, formatting, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }
    }
}