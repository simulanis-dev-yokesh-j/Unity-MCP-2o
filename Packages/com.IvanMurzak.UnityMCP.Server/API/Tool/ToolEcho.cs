using ModelContextProtocol.Server;
using System.ComponentModel;

namespace com.IvanMurzak.UnityMCP.Server.API.Tool
{
    [McpServerToolType]
    public static class EchoTool
    {
        [McpServerTool, Description("Echoes the message back to the client.")]
        public static string Echo(string message) => $"hello {message}";
    }
}