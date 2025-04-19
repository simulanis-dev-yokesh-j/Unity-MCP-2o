using System.Reflection;
using UnityEngine;
using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Common.Data.Unity;
using com.IvanMurzak.Unity.MCP.Common.Data.Utils;
using System.Linq;
using System;

namespace com.IvanMurzak.Unity.MCP.Utils
{
    /// <summary>
    /// Serializes Unity components to JSON format.
    /// </summary>
    public static partial class Serializer
    {
        public static class Component
        {
            /// <summary>
            /// Serializes the specified component to a JSON string.
            /// </summary>
            /// <param name="component">The component to serialize.</param>
            /// <returns>A JSON string representing the component's data.</returns>
            public static string Serialize(UnityEngine.Component component)
            {
                if (component == null)
                    return null;

                var list = BuildData(component);
                return JsonUtils.JsonSerialize(list);
            }
            public static string SerializeLight(UnityEngine.Component component)
            {
                if (component == null)
                    return null;

                var list = BuildDataLight(component);
                return JsonUtils.JsonSerialize(list);
            }
            public static ComponentDataLight BuildDataLight(UnityEngine.Component component)
            {
                if (component == null)
                    return null;

                return new ComponentDataLight()
                {
                    type = component.GetType().FullName,
                    isEnabled = BuildIsEnabled(component),
                    instanceId = component.GetInstanceID(),
                };
            }
            public static ComponentData.Enabled BuildIsEnabled(UnityEngine.Component component)
            {
                if (component == null)
                    return ComponentData.Enabled.NA;

                if (component is Behaviour behaviour)
                    return behaviour.enabled
                        ? ComponentData.Enabled.True
                        : ComponentData.Enabled.False;

                if (component is Renderer renderer)
                    return renderer.enabled
                        ? ComponentData.Enabled.True
                        : ComponentData.Enabled.False;

                return ComponentData.Enabled.NA;
            }
            public static ComponentData BuildData(UnityEngine.Component component)
            {
                if (component == null)
                    return null;

                var result = new ComponentData()
                {
                    type = component.GetType().FullName,
                    isEnabled = BuildIsEnabled(component),
                    instanceId = component.GetInstanceID()
                };
                var componentType = component.GetType();
                var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

                // Process fields
                foreach (var field in componentType.GetFields(flags)
                    .Where(field => field.GetCustomAttribute<ObsoleteAttribute>() == null)
                    .Where(field => field.IsPublic || field.IsPrivate && field.GetCustomAttribute<SerializeField>() != null))
                {
                    var value = field.GetValue(component);
                    var type = field.FieldType;

                    result.fields ??= new();

                    if (value == null)
                    {
                        result.fields.Add(SerializedMember.FromJson(field.Name, type, "null"));
                        continue;
                    }

                    // The type is a UnityEngine.Object type, so it should be handled differently.
                    var isUnityObject = typeof(UnityEngine.Object).IsAssignableFrom(type);
                    if (isUnityObject)
                    {
                        var unityObject = value as UnityEngine.Object;
                        var instanceIdJson = JsonUtility.ToJson(new InstanceId(unityObject.GetInstanceID()));
                        result.fields.Add(SerializedMember.FromJson(field.Name, type, instanceIdJson));
                    }
                    else
                    {
                        result.fields.Add(SerializedMember.FromJson(field.Name, type, JsonUtility.ToJson(value)));
                    }
                }

                // Process properties
                foreach (var prop in componentType.GetProperties(flags)
                    .Where(prop => prop.GetCustomAttribute<ObsoleteAttribute>() == null)
                    .Where(prop => prop.CanRead))
                {
                    try
                    {
                        var value = prop.GetValue(component);
                        var type = prop.PropertyType;

                        result.properties ??= new();

                        if (value == null)
                        {
                            result.properties.Add(SerializedMember.FromJson(prop.Name, type, "null"));
                            continue;
                        }

                        // The type is a UnityEngine.Object type, so it should be handled differently.
                        var isUnityObject = typeof(UnityEngine.Object).IsAssignableFrom(type);
                        if (isUnityObject)
                        {
                            var unityObject = value as UnityEngine.Object;
                            var instanceIdJson = JsonUtility.ToJson(new InstanceId(unityObject.GetInstanceID()));
                            result.properties.Add(SerializedMember.FromJson(prop.Name, type, instanceIdJson));
                        }
                        else
                        {
                            result.properties.Add(SerializedMember.FromJson(prop.Name, type, JsonUtility.ToJson(value)));
                        }
                    }
                    catch { /* skip inaccessible properties */ }
                }

                return result;
            }
        }
    }
}