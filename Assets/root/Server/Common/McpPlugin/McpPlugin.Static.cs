#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.Threading.Tasks;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public partial class McpPlugin : IMcpPlugin
    {
        static McpPlugin? instance;

        public static bool HasInstance => instance != null;
        public static IMcpPlugin? Instance => instance;

        public static Task StaticDisposeAsync()
        {
            var localInstance = instance;
            instance = null;

            return localInstance == null
                ? Task.CompletedTask
                : localInstance.DisposeAsync();
        }
    }
}