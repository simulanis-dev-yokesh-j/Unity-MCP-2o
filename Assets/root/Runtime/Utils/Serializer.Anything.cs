using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Common.Data.Unity;
using com.IvanMurzak.Unity.MCP.Common.Data.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace com.IvanMurzak.Unity.MCP.Utils
{
    /// <summary>
    /// Serializes Unity components to JSON format.
    /// </summary>
    public static partial class Serializer
    {
        public static class Anything
        {
            public static SerializedMember Serialize(object obj, BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            {
                var objType = obj.GetType();
                var isStruct = objType.IsValueType && !objType.IsPrimitive && !objType.IsEnum;

                if (objType.IsPrimitive)
                {
                    // Handle as primitive type
                    return SerializedMember.FromJson(string.Empty, objType, JsonUtility.ToJson(obj));
                }
                if (objType.IsEnum)
                {
                    // Handle as enum type
                    return SerializedMember.FromJson(string.Empty, objType, JsonUtility.ToJson(obj));
                }
                if (objType.IsClass || isStruct)
                {
                    // Handle as class or struct type
                    return new SerializedMember()
                    {
                        type = objType.FullName,
                        fields = SerializeFields(obj, flags),
                        properties = SerializeProperties(obj, flags),
                    };
                }

                throw new ArgumentException($"Unsupported type: {objType.FullName}");
            }

            public static List<SerializedMember> SerializeFields(object obj, BindingFlags flags)
            {
                var serialized = default(List<SerializedMember>);
                var objType = obj.GetType();

                foreach (var field in objType.GetFields(flags)
                    .Where(field => field.GetCustomAttribute<ObsoleteAttribute>() == null)
                    .Where(field => field.IsPublic || field.IsPrivate && field.GetCustomAttribute<SerializeField>() != null))
                {
                    var value = field.GetValue(obj);
                    var fieldType = field.FieldType;

                    serialized ??= new();

                    if (value == null)
                    {
                        serialized.Add(SerializedMember.FromJson(field.Name, fieldType, null));
                        continue;
                    }

                    // The type is a UnityEngine.Object type, so it should be handled differently.
                    var isUnityObject = typeof(UnityEngine.Object).IsAssignableFrom(fieldType);
                    if (isUnityObject)
                    {
                        var unityObject = value as UnityEngine.Object;
                        var instanceIDJson = JsonUtility.ToJson(new InstanceID(unityObject.GetInstanceID()));
                        serialized.Add(SerializedMember.FromJson(field.Name, fieldType, instanceIDJson));
                    }
                    else
                    {
                        serialized.Add(SerializedMember.FromJson(field.Name, fieldType, JsonUtility.ToJson(value)));
                    }
                }
                return serialized;
            }

            public static List<SerializedMember> SerializeProperties(object obj, BindingFlags flags)
            {
                var serialized = default(List<SerializedMember>);
                var objType = obj.GetType();

                foreach (var prop in objType.GetProperties(flags)
                    .Where(prop => prop.GetCustomAttribute<ObsoleteAttribute>() == null)
                    .Where(prop => prop.CanRead))
                {
                    try
                    {
                        var value = prop.GetValue(obj);
                        var propType = prop.PropertyType;

                        serialized ??= new();

                        if (value == null)
                        {
                            serialized.Add(SerializedMember.FromJson(prop.Name, propType, null));
                            continue;
                        }

                        // The type is a UnityEngine.Object type, so it should be handled differently.
                        var isUnityObject = typeof(UnityEngine.Object).IsAssignableFrom(propType);
                        if (isUnityObject)
                        {
                            var unityObject = value as UnityEngine.Object;
                            var instanceIDJson = JsonUtility.ToJson(new InstanceID(unityObject.GetInstanceID()));
                            serialized.Add(SerializedMember.FromJson(prop.Name, propType, instanceIDJson));
                        }
                        else
                        {
                            serialized.Add(SerializedMember.FromJson(prop.Name, propType, JsonUtility.ToJson(value)));
                        }
                    }
                    catch { /* skip inaccessible properties */ }
                }
                return serialized;
            }
        }
    }
}
