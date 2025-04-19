#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.Text.Json.Serialization;

namespace com.IvanMurzak.Unity.MCP.Common.Data.Utils
{
    [System.Serializable]
    public class InstanceId
    {
        [JsonInclude]
        [JsonPropertyName("instanceId")]
        public int instanceId;
        public InstanceId() { }
        public InstanceId(int id) => instanceId = id;
    }
}