#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using com.IvanMurzak.Unity.MCP.Common.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public static class JsonUtils
    {
        static JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, // Ignore null fields
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = false,
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };

        public static string ToJson(this IRequestData? data, JsonSerializerOptions? options = null)
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

        public static IRequestData? ParseRequestData(this string json, JsonSerializerOptions? options = null)
            => JsonSerializer.Deserialize<RequestData>(json, options ?? jsonSerializerOptions);

        public static IResponseData? ParseResponseData(this string json, JsonSerializerOptions? options = null)
            => JsonSerializer.Deserialize<ResponseData>(json, options ?? jsonSerializerOptions);

        public static class Resource
        {
            public static string ToJson(object data)
                => JsonSerializer.Serialize(data, jsonSerializerOptions);
        }
    }
}