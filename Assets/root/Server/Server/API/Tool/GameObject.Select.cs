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
            Name = "GameObject_Select",
            Title = "Select GameObjects in opened scene"
        )]
        [Description(@"Select GameObjects in opened scene by 'instanceID' (int) array.")]
        public Task<CallToolResponse> Find
        (
            [Description("The 'instanceID' array of the target GameObjects.")]
            int [] instanceIDs
        )
        {
            return ToolRouter.Call("GameObject_Select", arguments =>
            {
                arguments[nameof(instanceIDs)] = instanceIDs;
            });
        }
    }
}