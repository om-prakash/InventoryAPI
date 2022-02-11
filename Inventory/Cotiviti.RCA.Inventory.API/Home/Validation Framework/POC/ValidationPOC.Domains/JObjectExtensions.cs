using Newtonsoft.Json.Linq;

namespace ValidationPOC.Domains
{
    public static class JObjectExtensions
    {
        public static bool? GetBoolean(this JObject source, string key)
        {
            return source.Value<bool?>(key);
        }

        public static int? GetInteger(this JObject source, string key)
        {
            return source.Value<int?>(key);
        }

        public static string GetString(this JObject source, string key)
        {
            return source.Value<string>(key);
        }
    }
}
