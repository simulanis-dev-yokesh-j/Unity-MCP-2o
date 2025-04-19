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
        [Description(@"Get components of the target GameObject. Returns property values of each component.
Returns list of all available components preview if no requested components found.")]
        public Task<CallToolResponse> GetComponents
        (
            [Description("The 'instanceId' array of the target components. Leave it empty if all components needed.")]
            int[] componentInstanceIds,
            [Description("GameObject by 'instanceId'. Priority: 1. (Recommended)")]
            int? instanceId = null,
            [Description("GameObject by 'path'. Priority: 2.")]
            string? path = null,
            [Description("GameObject by 'name'. Priority: 3.")]
            string? name = null
        )
        {
            return ToolRouter.Call("GameObject_GetComponents", arguments =>
            {
                arguments[nameof(componentInstanceIds)] = componentInstanceIds;

                if (instanceId != null)
                    arguments[nameof(instanceId)] = instanceId;

                if (path != null)
                    arguments[nameof(path)] = path;

                if (name != null)
                    arguments[nameof(name)] = name;
            });
        }
    }
}