#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
namespace com.IvanMurzak.Unity.MCP.Common.Data.Unity
{
    [System.Serializable]
    public class GameObjectDataLight
    {
        public string Name { get; set; } = string.Empty;
        public string Tag { get; set; } = "Untagged";
        public int Layer { get; set; }
        public int InstanceId { get; set; }

        public GameObjectDataLight() { }
    }
}