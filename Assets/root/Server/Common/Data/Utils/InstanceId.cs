#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.Text.Json.Serialization;

namespace com.IvanMurzak.Unity.MCP.Common.Data.Utils
{
    [System.Serializable]
    public class InstanceID
    {
        [JsonInclude]
        [JsonPropertyName("instanceID")]
        public int instanceID;
        public InstanceID() { }
        public InstanceID(int id) => instanceID = id;
    }
}