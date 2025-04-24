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
            Title = "Modify Component at GameObject"
        )]
        [Description("Modify existed component at GameObject.")]
        public Task<CallToolResponse> ModifyComponent
        (

            [Description(@"Json Object with required readonly 'instanceID' and 'type' fields.
Any other field would be used for changing value in the target component.
Only required to modify properties and fields and with 'Type' field at the root.
It should respect the original structure of the component.
Nested 'instanceID' fields and properties are references to UnityEngine.Object types.
The target reference instance could be located in project assets, in the scene or in the prefabs.")]
            ComponentData data,
            [Description("GameObject by 'instanceID' (int). Priority: 1. (Recommended)")]
            int instanceID = 0,
            [Description("GameObject by 'path'. Priority: 2.")]
            string? path = null,
            [Description("GameObject by 'name'. Priority: 3.")]
            string? name = null
        )
        {
            return ToolRouter.Call("GameObject_ModifyComponent", arguments =>
            {
                arguments[nameof(data)] = data;
                arguments[nameof(instanceID)] = instanceID;

                if (path != null && path.Length > 0)
                    arguments[nameof(path)] = path;

                if (name != null && name.Length > 0)
                    arguments[nameof(name)] = name;
            });
        }
    }
}