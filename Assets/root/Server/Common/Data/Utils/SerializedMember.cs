#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace com.IvanMurzak.Unity.MCP.Common.Data.Utils
{
    [System.Serializable]
    public class SerializedMember
    {
        // UnityEngine.JsonUtility serialziation
        // --------------------------------------
        [JsonInclude] public string name = string.Empty; // needed for Unity's JsonUtility serialziation
        [JsonInclude] public string type = string.Empty; // needed for Unity's JsonUtility serialziation
        // public string? json;
        [JsonInclude] public List<SerializedMember>? fields; // needed for Unity's JsonUtility serialziation
        [JsonInclude] public List<SerializedMember>? properties; // needed for Unity's JsonUtility serialziation
                                                                 // --------------------------------------

        protected object? valueObject;
        [JsonInclude, JsonPropertyName("value")] public JsonElement? valueJsonElement;

        // System.Text.Json serialziation
        // --------------------------------------
        // [JsonPropertyName("name")]
        // public string? Name // needed for System.Text.Json serialziation
        // {
        //     get => name;
        //     set => name = value;
        // }
        // [JsonPropertyName("type")]
        // public string? Type // needed for System.Text.Json serialziation
        // {
        //     get => type;
        //     set => type = value;
        // }
        // [JsonPropertyName("value")]
        // public string? Json // needed for System.Text.Json serialziation
        // {
        //     get => json;
        //     set => json = value;
        // }
        // [JsonPropertyName("properties")]
        // public List<SerializedMember>? Properties // needed for System.Text.Json serialziation
        // {
        //     get => properties;
        //     set => properties = value;
        // }
        // --------------------------------------

        public SerializedMember() { }

        // protected SerializedMember(string name, object data)
        // {
        //     this.name = name;
        //     this.type = data.GetType().FullName;
        //     this.data = data;
        // }
        protected SerializedMember(string name, Type type, string json)
        {
            this.name = name;
            this.type = type.FullName ?? throw new ArgumentNullException(nameof(type));

            using var doc = JsonDocument.Parse(json);
            valueJsonElement = doc.RootElement.Clone();
        }

        // public static SerializedMember FromValue(string name, object data)
        //     => new SerializedMember(name, data);

        public static SerializedMember FromJson(string name, Type type, string json)
            => new SerializedMember(name, type, json);
    }
    // [System.Serializable]
    // public class SerializedMember<T> : SerializedMember
    // {
    //     // Incapsulated data
    //     // --------------------------------------
    //     protected T? value;
    //     protected JsonElement? dataJsonElement;
    //     // --------------------------------------

    //     public T? Value
    //     {
    //         get => value ??= dataJsonElement != null
    //             ? JsonUtils.Deserialize<T>(dataJsonElement.Value.GetRawText())
    //             : default;
    //         set
    //         {
    //             this.value = value;
    //             if (value != null)
    //             {
    //                 type = value.GetType().FullName;
    //                 json = JsonSerializer.Serialize(value, value.GetType(), JsonUtils.JsonSerializerOptions);
    //             }
    //         }
    //     }

    //     public SerializedMember() : base() { }

    //     // private SerializedMember(string name, object data)
    //     //     : base(name, data) { }
    //     private SerializedMember(string name, Type type, string json)
    //         : base(name, type, json) { }

    //     // public static new SerializedMember<T> FromValue(string name, object data)
    //     //     => new SerializedMember<T>(name, data);

    //     public static new SerializedMember<T> FromJson(string name, Type type, string json)
    //         => new SerializedMember<T>(name, type, json);
    // }
}