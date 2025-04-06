using com.IvanMurzak.UnityMCP.Common;
using com.IvanMurzak.UnityMCP.Common.API;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Threading.Tasks;

namespace com.IvanMurzak.UnityMCP.API.Editor
{
    public partial class Tool_GameObject : ServerTool
    {
        [McpServerTool(Name = "DeleteGameObject", Title = "Delete GameObject")]
        [Description("Delete a new GameObject in the current active scene.")]
        public Task<string> Delete(
            [Description("Full path (including name) to the parent GameObject.")]
            string fullPath)
        {
            return Execute(commandData => commandData
                .SetOrAddParameter(nameof(fullPath), fullPath));
        }
    }
}