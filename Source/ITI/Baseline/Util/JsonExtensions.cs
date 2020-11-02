using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITI.Baseline.Util
{
    public static class JsonExtensions
    {
        public static void ConsoleDump(this object obj, string? tag = null)
        {
            Dump(obj, tag, Console.WriteLine);
        }

        public static void Dump(this object obj, string? tag, Action<string> output)
        {
            var json = obj.ToJson();

            tag ??= GetTypeName(obj);
            output($"=== {tag} ===================================================");
            output(json);
        }

        private static string GetTypeName(object obj)
        {
            return (obj == null ? "(null)" : obj.GetType().Name);
        }

        public static string ToJson(this object obj, Formatting formatting = Formatting.Indented)
        {
            return JsonConvert.SerializeObject(obj, formatting, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ObjectCreationHandling = ObjectCreationHandling.Replace,
            });
        }

        public static string ToDbJson(this object obj)
        {
            var json = JsonConvert.SerializeObject(obj, Formatting.None, DbJsonSettings);
            return json;
        }

        public static T? FromDbJson<T>(this string json) where T : class
        {
            var entity = JsonConvert.DeserializeObject<T>(json, DbJsonSettings);
            return entity;
        }

        public static object? FromDbJson(this string json, Type t)
        {
            return JsonConvert.DeserializeObject(json, t, DbJsonSettings);
        }

        public static JsonSerializerSettings DbJsonSettings = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ObjectCreationHandling = ObjectCreationHandling.Replace,
            ContractResolver = new PrivateStateContractResolver()
        };
    }
}
