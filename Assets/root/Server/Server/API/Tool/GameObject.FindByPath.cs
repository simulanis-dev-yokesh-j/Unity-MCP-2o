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
            Name = "GameObject_FindByPath",
            Title = "Find GameObject by path"
        )]
        [Description("Find GameObject tree by the path to the root GameObject. Returns metadata about each GameObject.")]
        public Task<CallToolResponse> FindByPath
        (
            [Description("Path to the GameObject.")]
            string path,
            [Description("Determines the depth of the hierarchy to include.")]
            int includeChildrenDepth = 3
        )
        {
            return ToolRouter.Call("GameObject_FindByPath", arguments =>
            {
                arguments[nameof(path)] = path;
                arguments[nameof(includeChildrenDepth)] = includeChildrenDepth;
            });
        }
    }
}