using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ITI.Baseline.Util
{
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
            MemberSerialization memberSerialization
        )
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
}
