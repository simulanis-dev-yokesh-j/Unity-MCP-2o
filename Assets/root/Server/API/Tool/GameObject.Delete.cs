#if !IGNORE
using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Common.Server;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Threading.Tasks;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    public partial class Tool_GameObject : ServerTool
    {
        [McpServerTool(Name = "DeleteGameObject", Title = "Delete GameObject")]
        [Description("Delete a new GameObject in the current active scene.")]
        public Task<string> Delete(
            [Description("Full path (including name) to the parent GameObject.")]
            string fullPath)
        {
            return Execute(nameof(Delete), commandData => commandData
                .SetOrAddParameter(nameof(fullPath), fullPath));
        }
    }
}
#endif