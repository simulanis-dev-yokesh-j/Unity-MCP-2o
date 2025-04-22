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
            Name = "GameObject_SetParent",
            Title = "Set parent GameObject in opened scene"
        )]
        [Description(@"Duplicate GameObjects in opened scene by 'instanceId' (int) array.")]
        public Task<CallToolResponse> SetParent
        (
            [Description("The 'instanceId' array of the target GameObjects.")]
            int[] targetInstanceIds,
            [Description("The 'instanceId' of the parent GameObject.")]
            int parentInstanceId,
            [Description("The 'instanceId' of the parent GameObject.")]
            bool worldPositionStays = true
        )
        {
            return ToolRouter.Call("GameObject_SetParent", arguments =>
            {
                arguments[nameof(targetInstanceIds)] = targetInstanceIds ?? new int[0];
                arguments[nameof(parentInstanceId)] = parentInstanceId;
                arguments[nameof(worldPositionStays)] = worldPositionStays;
            });
        }
    }
}