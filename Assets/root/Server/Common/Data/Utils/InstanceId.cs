#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
namespace com.IvanMurzak.Unity.MCP.Common.Data.Utils
{
    [System.Serializable]
    public class InstanceId
    {
        public int instanceId;
        public int Id
        {
            get => instanceId;
            set => instanceId = value;
        }
        public InstanceId() { }
        public InstanceId(int id) => instanceId = id;
    }
}