using ModelContextProtocol.Protocol.Types;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Threading.Tasks;

namespace com.IvanMurzak.Unity.MCP.Server.API
{
    public partial class Tool_Scene
    {
        [McpServerTool
        (
            Name = "Scene_Unload",
            Title = "Unload scene"
        )]
        [Description("Name of the loaded scene.")]
        public Task<CallToolResponse> Save
        (
            [Description("Name of the loaded scene.")]
            string name
        )
        {
            return ToolRouter.Call("Scene_Unload", arguments =>
            {
                arguments[nameof(name)] = name;
            });
        }
    }
}