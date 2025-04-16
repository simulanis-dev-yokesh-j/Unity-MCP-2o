using ModelContextProtocol.Protocol.Types;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Threading.Tasks;

namespace com.IvanMurzak.Unity.MCP.Server.API
{
    public partial class Tool_GameObject
    {
        [McpServerTool
        (
            Name = "GameObject_GetComponents",
            Title = "Get GameObject components"
        )]
        [Description("Get all components of a GameObject by path. Returns property values of each component.")]
        public Task<CallToolResponse> GetComponents
        (
            [Description("Path to the GameObject.")]
            string path
        )
        {
            return ToolRouter.Call("GameObject_GetComponents", arguments =>
            {
                arguments[nameof(path)] = path;
            });
        }
    }
}