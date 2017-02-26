using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Kontur.GameStats.Server
{
    public class NamesContractResolver : DefaultContractResolver
    {
        public static readonly NamesContractResolver Instance = new NamesContractResolver();

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            // First letter to lowercase
            property.PropertyName =
                property.PropertyName[0].ToString().ToLower() +
                property.PropertyName.Substring(1);

            return property;
        }

        protected override string ResolvePropertyName(string propertyName)
        {
            string name = base.ResolvePropertyName(propertyName);

            // First letter to uppercase
            name =
                name[0].ToString().ToUpper() +
                name.Substring(1);

            return name;
        }


    }
}
