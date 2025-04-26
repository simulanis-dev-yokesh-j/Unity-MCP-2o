#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace com.IvanMurzak.Unity.MCP.Common.Data.Utils
{
    [Serializable]
    public class SerializedMember
    {
        [JsonInclude] public string name = string.Empty; // needed for Unity's JsonUtility serialization
        [JsonInclude] public string type = string.Empty; // needed for Unity's JsonUtility serialization
        [JsonInclude] public List<SerializedMember>? fields; // needed for Unity's JsonUtility serialization
        [JsonInclude] public List<SerializedMember>? properties; // needed for Unity's JsonUtility serialization

        [JsonInclude, JsonPropertyName("value")]
        public JsonElement? valueJsonElement = null; // System.Text.Json serialization

        public SerializedMember() { }

        protected SerializedMember(string name, Type type)
        {
            this.name = name;
            this.type = type.FullName ?? throw new ArgumentNullException(nameof(type));
        }

        public static SerializedMember FromJson(string name, Type type, string json)
        {
            var result = new SerializedMember(name, type);
            using (var doc = JsonDocument.Parse(json))
            {
                result.valueJsonElement = doc.RootElement.Clone();
            }
            return result;
        }
        public static SerializedMember FromPrimitive(string name, Type type, object primitiveValue)
        {
            var result = new SerializedMember(name, type);
            var json = JsonSerializer.Serialize(primitiveValue);
            using (var doc = JsonDocument.Parse(json))
            {
                result.valueJsonElement = doc.RootElement.Clone();
            }
            return result;
        }
    }
}