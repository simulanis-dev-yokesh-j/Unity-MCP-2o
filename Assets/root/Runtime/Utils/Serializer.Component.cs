using System.Reflection;
using UnityEngine;
using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Common.Data.Unity;
using com.IvanMurzak.Unity.MCP.Common.Data.Utils;

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
                    Type = component.GetType().FullName,
                    IsEnabled = component is MonoBehaviour mh
                        ? mh.enabled
                            ? ComponentData.Enabled.True
                            : ComponentData.Enabled.False
                        : ComponentData.Enabled.NA,
                    InstanceId = component.GetInstanceID(),
                };
            }
            public static ComponentData BuildData(UnityEngine.Component component)
            {
                if (component == null)
                    return null;

                var result = new ComponentData()
                {
                    Type = component.GetType().FullName,
                    IsEnabled = component is MonoBehaviour mh
                        ? mh.enabled
                            ? ComponentData.Enabled.True
                            : ComponentData.Enabled.False
                        : ComponentData.Enabled.NA,
                    InstanceId = component.GetInstanceID(),
                    Properties = new()
                };
                var type = component.GetType();
                var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

                foreach (var field in type.GetFields(flags))
                {
                    var value = field.GetValue(component);
                    result.Properties.Add(new(field.Name, value.GetType().FullName, JsonUtility.ToJson(value)));
                }

                foreach (var prop in type.GetProperties(flags))
                {
                    if (prop.CanRead)
                    {
                        try
                        {
                            var value = prop.GetValue(component);
                            if (value is UnityEngine.Object obj)
                            {
                                result.Properties.Add(new(prop.Name, value.GetType().FullName, JsonUtility.ToJson(new InstanceId(obj.GetInstanceID()))));
                                continue;
                            }
                            result.Properties.Add(new(prop.Name, value.GetType().FullName, JsonUtility.ToJson(value)));
                        }
                        catch { /* skip inaccessible properties */ }
                    }
                }

                return result;
            }
        }
    }
}