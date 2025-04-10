#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.Collections.Generic;

namespace com.IvanMurzak.Unity.MCP.Common.Data.Unity
{
    [System.Serializable]
    public class GameObjectData
    {
        public string Name { get; set; } = string.Empty;
        public string Tag { get; set; } = "Untagged";
        public int Layer { get; set; }
        public int InstanceId { get; set; }
        public List<ComponentData> Components { get; set; } = new();

        public GameObjectData() { }
    }
}