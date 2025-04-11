#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
namespace com.IvanMurzak.Unity.MCP.Common
{
    public partial class McpApp
    {
        public enum Status
        {
            Disconnected = 0,
            Connecting = 1,
            Connected = 2,
            Disconnecting = 3,
            Error = 4
        }
        public enum ConnectionRole
        {
            Unity = 0,
            Server = 1
        }
    }
}