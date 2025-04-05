using com.IvanMurzak.UnityMCP.Common.API;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Threading.Tasks;

namespace com.IvanMurzak.UnityMCP.Server.API.Tool
{
    [McpServerToolType]
    public static class ToolGameObject
    {
        [McpServerTool, Description("Creates gameObject at specific path with specific name")]
        public static Task<string?> Create(string path, string name)
            => Connector.Instance?.Send($"CreateGameObject {path} {name}") ?? Task.FromResult<string?>(null);
    }
}