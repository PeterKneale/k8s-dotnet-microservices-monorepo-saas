using Newtonsoft.Json;

namespace Carts.Infrastructure.Json
{
    public static class CustomJsonSerializer
    {
        public static string ToJson<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        
        public static T? FromJson<T>(this string? json)
        {
            return json == null ? default : JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
            {
                ContractResolver = new CustomJsonContractResolver()
            });
        }
    }
}