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
            Name = "Scene_Open",
            Title = "Open scene"
        )]
        [Description("Open scene from the project assets.")]
        public Task<CallToolResponse> Open
        (
            [Description("Path to the scene file.")]
            string path,
            [Description("Open scene mode. 0 - Single, 1 - Additive.")]
            int openSceneMode = 0
        )
        {
            return ToolRouter.Call("Scene_Open", arguments =>
            {
                arguments[nameof(path)] = path;
                arguments[nameof(openSceneMode)] = openSceneMode;
            });
        }
    }
}