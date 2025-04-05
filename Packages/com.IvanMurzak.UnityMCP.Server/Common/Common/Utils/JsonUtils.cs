using com.IvanMurzak.UnityMCP.Common.Data;
using System.Text.Json;

namespace com.IvanMurzak.UnityMCP.Common
{
    public static class JsonUtils
    {
        static JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            Converters =
            {
                new System.Text.Json.Serialization.JsonStringEnumConverter()
            }
        };

        public static string ToJson(this IDataPackage? data, JsonSerializerOptions? options = null)
        {
            if (data == null)
                return "{}";

            return JsonSerializer.Serialize(data, options ?? jsonSerializerOptions);
        }

        public static string ToJson(this IResponseData? data, JsonSerializerOptions? options = null)
        {
            if (data == null)
                return "{}";

            return JsonSerializer.Serialize(data, options ?? jsonSerializerOptions);
        }

        public static IDataPackage? ParseDataPackage(this string json, JsonSerializerOptions? options = null)
        {
            return JsonSerializer.Deserialize<IDataPackage>(json, options ?? jsonSerializerOptions);
        }

        public static IResponseData? ParseResponseData(this string json, JsonSerializerOptions? options = null)
        {
            return JsonSerializer.Deserialize<IResponseData>(json, options ?? jsonSerializerOptions);
        }
    }
}