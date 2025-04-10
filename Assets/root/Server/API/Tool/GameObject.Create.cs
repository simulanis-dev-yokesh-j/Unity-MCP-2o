#if !IGNORE
using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Common.Server;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Threading.Tasks;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    [McpServerToolType]
    public partial class Tool_GameObject : ServerTool
    {
        [McpServerTool(Name = "GameObject_Create", Title = "Create GameObject")]
        [Description("Create a new GameObject in the current active scene.")]
        public Task<string> Create(
            [Description("Path to the parent GameObject.")]
            string path,
            [Description("Name of the new GameObject.")]
            string name)
        {
            return Execute(nameof(Create), commandData => commandData
                .SetOrAddParameter(nameof(path), path)
                .SetOrAddParameter(nameof(name), name));
        }
    }
}
#endif