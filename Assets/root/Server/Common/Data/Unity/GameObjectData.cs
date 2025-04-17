#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.Collections.Generic;

namespace com.IvanMurzak.Unity.MCP.Common.Data.Unity
{
    [System.Serializable]
    public class GameObjectData : GameObjectDataLight
    {
        public List<ComponentData> components { get; set; } = new();

        public GameObjectData() { }
    }
}