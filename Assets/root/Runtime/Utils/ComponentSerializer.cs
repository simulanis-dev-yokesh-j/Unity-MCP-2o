using System.Reflection;
using UnityEngine;
using System.Collections.Generic;

namespace com.IvanMurzak.Unity.Runtime
{
    /// <summary>
    /// Serializes Unity components to JSON format.
    /// </summary>
    public static class ComponentSerializer
    {
        public static string Serialize(params Component[] components)
        {
            var stringBuilder = new System.Text.StringBuilder("[");
            for (int i = 0; i < components.Length; i++)
            {
                var component = components[i];
                if (component == null)
                    continue;

                stringBuilder.Append(Serialize(component));
                if (i < components.Length - 1)
                    stringBuilder.Append(",");
            }
            stringBuilder.Append("]");
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Serializes the specified component to a JSON string.
        /// </summary>
        /// <param name="component">The component to serialize.</param>
        /// <returns>A JSON string representing the component's data.</returns>
        public static string Serialize(Component component)
        {
            if (component == null)
                return "{}";
            var dict = new Dictionary<string, object>();
            var type = component.GetType();
            var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            foreach (var field in type.GetFields(flags))
            {
                var value = field.GetValue(component);
                dict[field.Name] = value;
            }

            foreach (var prop in type.GetProperties(flags))
            {
                if (prop.CanRead && prop.GetIndexParameters().Length == 0)
                {
                    try
                    {
                        var value = prop.GetValue(component);
                        dict[prop.Name] = value;
                    }
                    catch { /* skip inaccessible properties */ }
                }
            }

            return JsonUtility.ToJson(new SerializationWrapper(dict), true);
        }

        [System.Serializable]
        private class SerializationWrapper
        {
            public Dictionary<string, object> data;
            public SerializationWrapper(Dictionary<string, object> d) => data = d;
        }
    }
}
