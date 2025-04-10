using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Common.Data.Unity;
using System.Linq;

namespace com.IvanMurzak.Unity.Runtime
{
    /// <summary>
    /// Serializes Unity components to JSON format.
    /// </summary>
    public static partial class Serializer
    {
        public static class GameObject
        {
            public static GameObjectData BuildData(UnityEngine.GameObject go)
            {
                if (go == null)
                    return null;

                return new GameObjectData()
                {
                    Name = go.name,
                    Tag = go.tag,
                    Layer = go.layer,
                    InstanceId = go.GetInstanceID(),
                    Components = go.GetComponents<UnityEngine.Component>()
                        .Select(c => Component.BuildData(c))
                        .Where(c => c != null)
                        .ToList()
                };
            }
            public static string Serialize(UnityEngine.GameObject go)
            {
                var jsonResult = JsonUtils.JsonSerialize(BuildData(go));
                // Debug.Log($"{go.name}.{go.GetType().Name} : {jsonResult}");
                return jsonResult;
            }
        }
    }
}
