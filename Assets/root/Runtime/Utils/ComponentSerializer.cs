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

                var json = Serialize(component);
                if (string.IsNullOrEmpty(json))
                    continue;

                stringBuilder.Append(json);
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
                return null;

            var list = InternalSerialize(component);
            var json = JsonUtility.ToJson(list);
            Debug.Log($"{component.name}.{component.GetType().Name} : {json}");
            return json;
            // return JsonUtils.JsonSerialize(InternalSerialize(component));
        }
        static List<Pair> InternalSerialize(Component component)
        {
            if (component == null)
                return null;

            var dict = new List<Pair>();
            var type = component.GetType();
            var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            foreach (var field in type.GetFields(flags))
            {
                var value = field.GetValue(component);
                dict.Add(new Pair(field.Name, value));
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
                            dict.Add(new Pair(prop.Name, new InstanceId(obj)));
                            continue;
                        }
                        dict.Add(new Pair(prop.Name, value));
                    }
                    catch { /* skip inaccessible properties */ }
                }
            }

            return dict;
        }
    }
    [System.Serializable]
    public class InstanceId
    {
        public int instanceId;
        public InstanceId() { }
        public InstanceId(int id) => instanceId = id;
        public InstanceId(UnityEngine.Object obj) : this(obj.GetInstanceID()) { }
    }
    [System.Serializable]
    public class Pair
    {
        public string key;
        public object obj;
        public Pair(string key, object obj)
        {
            this.key = key;
            this.obj = obj;
        }
    }
}
