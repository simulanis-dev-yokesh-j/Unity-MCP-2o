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
        [JsonInclude] public string name = string.Empty; // needed for Unity's JsonUtility serialziation
        [JsonInclude] public string type = string.Empty; // needed for Unity's JsonUtility serialziation
        [JsonInclude] public List<SerializedMember>? fields; // needed for Unity's JsonUtility serialziation
        [JsonInclude] public List<SerializedMember>? properties; // needed for Unity's JsonUtility serialziation

        [JsonInclude, JsonPropertyName("value")]
        public JsonElement? valueJsonElement = null; // System.Text.Json serialization

        public SerializedMember() { }

        protected SerializedMember(string name, Type type, string json)
        {
            this.name = name;
            this.type = type.FullName ?? throw new ArgumentNullException(nameof(type));

            using var doc = JsonDocument.Parse(json);
            valueJsonElement = doc.RootElement.Clone();
        }

        public static SerializedMember FromJson(string name, Type type, string json)
            => new SerializedMember(name, type, json);
    }
}