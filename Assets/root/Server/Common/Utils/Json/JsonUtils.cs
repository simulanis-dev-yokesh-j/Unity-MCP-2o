#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using com.IvanMurzak.Unity.MCP.Common.Data;
using com.IvanMurzak.Unity.MCP.Common.Json;
using System;
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
            //ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true,
            Converters =
            {
                new JsonStringEnumConverter(),
                new SerializedMemberConverter(),
                new InstanceIdConverter()
            }
        };

        public static void AddConverter(JsonConverter converter)
        {
            if (converter == null)
                throw new ArgumentNullException(nameof(converter));

            jsonSerializerOptions.Converters.Add(converter);
        }

        public static string ToJson(this IRequestCallTool? data, JsonSerializerOptions? options = null)
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

        public static string JsonSerialize(this object? data, JsonSerializerOptions? options = null)
        {
            if (data == null)
                return "null";

            return JsonSerializer.Serialize(data, options ?? jsonSerializerOptions);
        }

        public static IRequestCallTool? ParseRequestData(this string json, JsonSerializerOptions? options = null)
            => JsonSerializer.Deserialize<IRequestCallTool>(json, options ?? jsonSerializerOptions);

        public static IResponseData? ParseResponseData(this string json, JsonSerializerOptions? options = null)
            => JsonSerializer.Deserialize<ResponseData>(json, options ?? jsonSerializerOptions);

        public static class Resource
        {
            public static string ToJson(object data)
                => JsonSerializer.Serialize(data, jsonSerializerOptions);
        }
    }
}