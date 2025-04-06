#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
namespace com.IvanMurzak.Unity.MCP.Common
{
    public partial class Connector : IConnector
    {
        static Connector? instance;

        public static bool HasInstance => instance != null;
        public static Connector? Instance => instance;
    }
}