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
                    isEnabled = component is MonoBehaviour mh
                        ? mh.enabled
                            ? ComponentData.Enabled.True
                            : ComponentData.Enabled.False
                        : ComponentData.Enabled.NA,
                    instanceId = component.GetInstanceID(),
                };
            }
            public static ComponentData BuildData(UnityEngine.Component component)
            {
                if (component == null)
                    return null;

                var result = new ComponentData()
                {
                    type = component.GetType().FullName,
                    isEnabled = component is Behaviour behaviour
                        ? behaviour.enabled
                            ? ComponentData.Enabled.True
                            : ComponentData.Enabled.False
                        : ComponentData.Enabled.NA,
                    instanceId = component.GetInstanceID(),
                    properties = new()
                };
                var type = component.GetType();
                var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

                foreach (var field in type.GetFields(flags))
                {
                    var value = field.GetValue(component);
                    // result.properties.Add(new(field.Name, value));
                    // result.properties.Add(new(field.Name, JsonUtility.ToJson(value)));

                    result.properties.Add(SerializedMember.FromJson(field.Name, value.GetType(), value is UnityEngine.Object obj
                        ? JsonUtility.ToJson(new InstanceId(obj.GetInstanceID()))
                        : JsonUtility.ToJson(value)));
                }

                foreach (var prop in type.GetProperties(flags)
                    .Where(prop => prop.GetCustomAttribute<ObsoleteAttribute>() == null)
                    .Where(prop => prop.CanRead))
                {
                    try
                    {
                        var value = prop.GetValue(component);
                        // result.properties.Add(new(prop.Name, obj));
                        // result.properties.Add(new(prop.Name, JsonUtility.ToJson(new InstanceId(obj.GetInstanceID()))));
                        // result.properties.Add(new(prop.Name, value));
                        // result.properties.Add(new(prop.Name, JsonUtility.ToJson(value)));

                        result.properties.Add(SerializedMember.FromJson(prop.Name, value.GetType(), value is UnityEngine.Object obj
                            ? JsonUtility.ToJson(new InstanceId(obj.GetInstanceID()))
                            : JsonUtility.ToJson(value)));
                    }
                    catch { /* skip inaccessible properties */ }
                }

                return result;
            }
        }
    }
}