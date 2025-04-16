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
        [Description(@"Destroy a GameObject and all nested GameObjects recursively.
Use 'instanceId' whenever possible, because it finds the exact GameObject, when 'path' may find a wrong one.")]
        public ValueTask<CallToolResponse> Destroy
        (
            [Description("Delete by 'instanceId'. Priority: 1. (Recommended)")]
            int? instanceId = null,
            [Description("Delete by 'path'. Priority: 2.")]
            string? path = null,
            [Description("Delete by 'name'. Priority: 3.")]
            string? name = null
        )
        {
            return ToolRouter.Call("GameObject_Destroy", arguments =>
            {
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