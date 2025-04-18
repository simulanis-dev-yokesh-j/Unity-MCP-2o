#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace com.IvanMurzak.Unity.MCP.Common.Data.Utils
{
    [System.Serializable]
    public class SerializedMember
    {
        public string? name;
        public string? type;
        public string? json; // written UnityEngine.Object exnteded class value as json string usint JsonUtility.ToJson(value)
        public object? value; //
        public JsonElement? valueJsonElement;
        public List<SerializedMember>? properties;

        // Does it needed for Json serialization?
        // --------------------------------------
        // public string? Name
        // {
        //     get => name;
        //     set => name = value;
        // }
        // public string? Type
        // {
        //     get => type;
        //     set => type = value;
        // }
        // public string? Json
        // {
        //     get => json;
        //     set => json = value;
        // }
        // public List<SerializedMember>? Properties
        // {
        //     get => properties;
        //     set => properties = value;
        // }
        // --------------------------------------

        public SerializedMember() { }

        private SerializedMember(string name, Type type, string json)
        {
            this.name = name;
            this.type = type.FullName;
            this.json = json;
        }
        private SerializedMember(string name, object value)
        {
            this.name = name;
            this.type = value.GetType().FullName;
            this.value = value;
        }

        public static SerializedMember FromValue(string name, object value)
            => new SerializedMember(name, value);

        public static SerializedMember FromJson(string name, Type type, string json)
            => new SerializedMember(name, type, json);
    }
}