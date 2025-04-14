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
            Name = "Scene_GetHierarchyRoot",
            Title = "Get Scene Hierarchy"
        )]
        [Description("This tool retrieves the list of root GameObjects in the specified scene.")]
        public Task<CallToolResponse> GetHierarchyRoot
        (
            [Description("Determines the depth of the hierarchy to include.")]
            int includeChildrenDepth = 3,
            [Description("Name of the loaded scene. If empty, the active scene will be used.")]
            string? loadedSceneName = null
        )
        {
            return ToolRouter.Call("Scene_GetHierarchyRoot", arguments =>
            {
                arguments[nameof(includeChildrenDepth)] = includeChildrenDepth;
                arguments[nameof(loadedSceneName)] = loadedSceneName ?? string.Empty;
            });
        }
    }
}