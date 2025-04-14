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
            Name = "GameObject_FindByName",
            Title = "Find GameObject by name"
        )]
        [Description("Find GameObject by name in the active scene. Returns the path to the GameObject.")]
        public Task<CallToolResponse> FindByName
        (
            [Description("Name of the target GameObject.")]
            string name,
            [Description("Include children GameObjects in the result.")]
            bool includeChildren = true,
            [Description("Include children GameObjects recursively in the result. Ignored if 'includeChildren' is false.")]
            bool includeChildrenRecursively = false
        )
        {
            return ToolRouter.Call("GameObject_FindByName", arguments =>
            {
                arguments[nameof(name)] = name;
                arguments[nameof(includeChildren)] = includeChildren;
                arguments[nameof(includeChildrenRecursively)] = includeChildrenRecursively;
            });
        }
    }
}