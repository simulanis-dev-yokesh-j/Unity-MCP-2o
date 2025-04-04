using ModelContextProtocol.Server;
using System.ComponentModel;

namespace com.IvanMurzak.UnityMCP.Server.API.Tool
{
    [McpServerToolType]
    public static class ToolGameObject
    {
        [McpServerTool, Description("Creates gameObject at specific path with specific name")]
        public static string Create(string path, string name) => $"Create at {path} with name {name}";
    }
}