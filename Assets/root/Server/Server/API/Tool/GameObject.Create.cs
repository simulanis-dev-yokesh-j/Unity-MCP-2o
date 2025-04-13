using com.IvanMurzak.Unity.MCP.Server;
using ModelContextProtocol.Protocol.Types;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Threading.Tasks;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    public partial class Tool_GameObject
    {
        [McpServerTool
        (
            Name = "GameObject_Create",
            Title = "Create GameObject"
        )]
        [Description("Create a new GameObject in the current active scene.")]
        public Task<CallToolResponse> Create
        (
            [Description("Path to the parent GameObject.")]
            string path,
            [Description("Name of the new GameObject.")]
            string name
        )
        {
            return ToolRouter.Call("GameObject_Create", arguments =>
            {
                arguments[nameof(path)] = path;
                arguments[nameof(name)] = name;
            });
        }
    }
}