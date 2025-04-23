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
        public Task<CallToolResponse> Destroy
        (
            [Description("Delete by 'instanceId' (int). Priority: 1. (Recommended)")]
            int instanceId = 0,
            [Description("Delete by 'path'. Priority: 2.")]
            string? path = null,
            [Description("Delete by 'name'. Priority: 3.")]
            string? name = null
        )
        {
            return ToolRouter.Call("GameObject_Destroy", arguments =>
            {
                arguments[nameof(instanceId)] = instanceId;

                if (path != null && path.Length > 0)
                    arguments[nameof(path)] = path;

                if (name != null && name.Length > 0)
                    arguments[nameof(name)] = name;
            });
        }
    }
}