using ModelContextProtocol.Protocol.Types;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Threading.Tasks;

namespace com.IvanMurzak.Unity.MCP.Server.API
{
    public partial class Tool_Assets_Prefab
    {
        [McpServerTool
        (
            Name = "Assets_Prefab_Instantiate",
            Title = "Instantiate prefab in the current active scene"
        )]
        [Description("Instantiates prefab in a scene.")]
        public ValueTask<CallToolResponse> Instantiate
        (
            [Description("Prefab asset path.")]
            string prefabAssetPath,
            [Description("GameObject path in the current active scene.")]
            string gameObjectPath
        )
        {
            return ToolRouter.Call("Assets_Prefab_Instantiate", arguments =>
            {
                arguments[nameof(prefabAssetPath)] = prefabAssetPath;
                arguments[nameof(gameObjectPath)] = gameObjectPath;
            });
        }
    }
}