using com.IvanMurzak.Unity.MCP.Common.Data.Unity;
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
            Name = "GameObject_ModifyComponent",
            Title = "Add Component to a GameObject"
        )]
        [Description("Add a component to a GameObject.")]
        public Task<CallToolResponse> ModifyComponent
        (
            [Description("Json Object with required readonly 'instanceId' and 'type' fields. Any other field would be used for changing value in the target component. only required to modify properties and fields and with 'Type' field at the root. It should respect the original structure of the component.")]
            ComponentData data,
            [Description("GameObject by 'instanceId'. Priority: 1. (Recommended)")]
            int? instanceId = null,
            [Description("GameObject by 'path'. Priority: 2.")]
            string? path = null,
            [Description("GameObject by 'name'. Priority: 3.")]
            string? name = null
        )
        {
            return ToolRouter.Call("GameObject_ModifyComponent", arguments =>
            {
                arguments[nameof(data)] = data;

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