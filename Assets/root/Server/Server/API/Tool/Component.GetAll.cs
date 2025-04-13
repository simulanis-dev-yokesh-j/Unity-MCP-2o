using com.IvanMurzak.Unity.MCP.Server;
using ModelContextProtocol.Protocol.Types;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Threading.Tasks;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    public partial class Tool_Component
    {
        [McpServerTool(Name = "Component_Get_All", Title = "Add Component")]
        [Description("Add new Component instance to a target GameObject.")]
        public Task<CallToolResponse> Add(
            [Description("Path to the GameObject.")]
            string path,
            [Description("Component class full name.")]
            string fullName)
        {
            return ToolRouter.Call("Component_Get_All", arguments =>
            {
                arguments[nameof(path)] = path;
                arguments[nameof(fullName)] = fullName;
            });
        }
    }
}