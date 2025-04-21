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
            Name = "GameObject_Duplicate",
            Title = "Duplicate GameObjects in opened scene"
        )]
        [Description(@"Duplicate GameObjects in opened scene by 'instanceId' (int) array.")]
        public Task<CallToolResponse> Duplicate
        (
            [Description("The 'instanceId' array of the target GameObjects.")]
            int [] instanceIds
        )
        {
            return ToolRouter.Call("GameObject_Duplicate", arguments =>
            {
                arguments[nameof(instanceIds)] = instanceIds;
            });
        }
    }
}