using ModelContextProtocol.Protocol.Types;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Threading.Tasks;

namespace com.IvanMurzak.Unity.MCP.Server.API
{
    public partial class Tool_Assets
    {
        [McpServerTool
        (
            Name = "Assets_Refresh",
            Title = "Assets Refresh"
        )]
        [Description(@"Refreshes the AssetDatabase. Use it if any new files were added or updated in the project. It would also trigger recompilation of the scripts.")]
        public Task<CallToolResponse> Refresh()
        {
            return ToolRouter.Call("Assets_Search", arguments =>
            {
                arguments[nameof(filter)] = filter ?? string.Empty;
                arguments[nameof(searchInFolders)] = searchInFolders ?? new string[0];
            });
        }
    }
}