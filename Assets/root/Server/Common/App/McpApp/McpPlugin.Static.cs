#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
namespace com.IvanMurzak.Unity.MCP.Common
{
    public partial class McpPlugin : IMcpPlugin
    {
        static McpPlugin? instance;

        public static bool HasInstance => instance != null;
        public static IMcpPlugin? Instance => instance;
        // public static IConnectorServer? Server => instance;
        // public static IConnectorApp? App => instance;

        public static void StaticDispose()
        {
            instance?.Dispose();
            instance = null;
        }
    }
}