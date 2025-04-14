using ModelContextProtocol.Protocol.Types;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Threading.Tasks;

namespace com.IvanMurzak.Unity.MCP.Server.API
{
    public partial class Tool_Assets_Prefab
    {
        [McpServerTool
        (
            Name = "Assets_Prefabs_GetAll",
            Title = "Get list of all prefabs in the project"
        )]
        [Description("Returns the list of all available prefabs in the project.")]
        public Task<CallToolResponse> GetAll
        (
            [Description("Substring for searching prefabs. Could be empty.")]
            string search
        )
        {
            return ToolRouter.Call("Assets_Prefabs_GetAll", arguments =>
            {
                arguments[nameof(search)] = search;
            });
        }
    }
}