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
            Name = "GameObject_Destroy",
            Title = "Destroy GameObject"
        )]
        [Description("Destroy GameObject in the current active scene.")]
        public Task<CallToolResponse> Destroy
        (
            [Description("Full path (including name) to the target GameObject.")]
            string fullPath
        )
        {
            return ToolRouter.Call("GameObject_Destroy", arguments =>
            {
                arguments[nameof(fullPath)] = fullPath;
            });
        }
    }
}