using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Iti.Baseline.Utilities
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

            tag = tag == null ? GetTypeName(obj) : $"{tag}";
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

        public static T FromDbJson<T>(this string json)
        {
            var entity = JsonConvert.DeserializeObject<T>(json, DbJsonSettings);
            return entity;
        }

        public static object FromDbJson(this string json, Type t)
        {
            return JsonConvert.DeserializeObject(json, t, DbJsonSettings);
        }

        public class PrivateStateContractResolver : CamelCasePropertyNamesContractResolver
        {
            protected override List<MemberInfo> GetSerializableMembers(Type objectType)
            {
                const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
                var properties = objectType.GetProperties(bindingFlags);
                var fields = objectType.GetFields(bindingFlags);

                var allMembers = properties.Cast<MemberInfo>().Union(fields);
                return allMembers.ToList();
            }

            protected override JsonProperty CreateProperty(
                MemberInfo member,
                MemberSerialization memberSerialization)
            {
                var prop = base.CreateProperty(member, memberSerialization);

                if (!prop.Writable)
                {
                    var property = member as PropertyInfo;
                    if (property != null)
                    {
                        var hasPrivateSetter = property.GetSetMethod(true) != null;
                        prop.Writable = hasPrivateSetter;
                    }
                }

                return prop;
            }
        }

        public static JsonSerializerSettings DbJsonSettings = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ObjectCreationHandling = ObjectCreationHandling.Replace,
            ContractResolver = new PrivateStateContractResolver()
        };
    }
}