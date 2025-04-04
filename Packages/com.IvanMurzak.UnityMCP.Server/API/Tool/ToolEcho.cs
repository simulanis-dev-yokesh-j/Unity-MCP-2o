using com.IvanMurzak.UnityMCP.Common.API;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Threading.Tasks;

namespace com.IvanMurzak.UnityMCP.Server.API.Tool
{
    [McpServerToolType]
    public static class EchoTool
    {
        [McpServerTool, Description("Echoes the message back to the client.")]
        public static async Task<string> Echo(string message)
        {
            await Connector.Instance.Send($"hello {message}");
            return "Operation completed";
        }
    }
}