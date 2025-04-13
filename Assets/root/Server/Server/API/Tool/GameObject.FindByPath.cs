using com.IvanMurzak.Unity.MCP.Server;
using ModelContextProtocol.Protocol.Types;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Threading.Tasks;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    public partial class Tool_GameObject
    {
        [McpServerTool
        (
            Name = "GameObject_FindByPath",
            Title = "Find GameObject by path"
        )]
        [Description("Find GameObject tree by the path to the root GameObject. Returns metadata about each GameObject.")]
        public Task<CallToolResponse> FindByPath
        (
            [Description("Path to the GameObject.")]
            string path,
            [Description("Include children GameObjects in the result.")]
            bool includeChildren = true,
            [Description("Include children GameObjects recursively in the result. Ignored if 'includeChildren' is false.")]
            bool includeChildrenRecursively = false
        )
        {
            return ToolRouter.Call("GameObject_FindByPath", arguments =>
            {
                arguments[nameof(path)] = path;
                arguments[nameof(includeChildren)] = includeChildren;
                arguments[nameof(includeChildrenRecursively)] = includeChildrenRecursively;
            });
        }
    }
}