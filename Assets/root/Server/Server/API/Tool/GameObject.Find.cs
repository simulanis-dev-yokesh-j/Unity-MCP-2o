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
            Name = "GameObject_FindByInstanceId",
            Title = "Find GameObject by InstanceId"
        )]
        [Description("Find GameObject in the active scene. Returns metadata about GameObject and its children.")]
        public Task<CallToolResponse> FindByInstanceId
        (
            [Description("Determines the depth of the hierarchy to include. 0 - means only the target GameObject. 1 - means to include one layer below.")]
            int includeChildrenDepth = 0,
            [Description("Find by 'instanceId'. Priority: 1. (Recommended)")]
            int? instanceId = null,
            [Description("Find by 'path'. Priority: 2.")]
            string? path = null,
            [Description("Find by 'name'. Priority: 3.")]
            string? name = null
        )
        {
            return ToolRouter.Call("GameObject_FindByInstanceId", arguments =>
            {
                arguments[nameof(includeChildrenDepth)] = includeChildrenDepth;

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