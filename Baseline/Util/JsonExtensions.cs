using System.Text.Json;

namespace ITI.Baseline.Util
{
    public static class JsonExtensions
    {
        public static void ConsoleDump(this object obj, string? tag = null)
        {
            Dump(obj, tag, Console.WriteLine);
        }

        private static readonly JsonSerializerOptions DumpSerializerOptions = new()
        {
            WriteIndented = true
        };

        public static void Dump(this object obj, string? tag, Action<string> output)
        {
            var json = JsonSerializer.Serialize(obj, DumpSerializerOptions);

            tag ??= GetTypeName(obj);
            output($"=== {tag} ===================================================");
            output(json);
        }

        private static string GetTypeName(object obj)
        {
            return obj == null ? "(null)" : obj.GetType().Name;
        }

        private static readonly JsonSerializerOptions DbJsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public static string ToDbJson(this object obj)
        {
            return JsonSerializer.Serialize(obj, DbJsonSerializerOptions);
        }

        public static T? FromDbJson<T>(this string json)
            where T : class
        {
            return JsonSerializer.Deserialize<T>(json, DbJsonSerializerOptions);
        }

        public static object? FromDbJson(this string json, Type t)
        {
            return JsonSerializer.Deserialize(json, t, DbJsonSerializerOptions);
        }
    }
}
