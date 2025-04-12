#if !IGNORE
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Threading.Tasks;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    [McpServerToolType]
    public partial class Tool_Component //  : ServerTool
    {
        [McpServerTool(Name = "Component_Add", Title = "Add Component")]
        [Description("Add new Component instance to a target GameObject.")]
        public Task<string> Add(
            [Description("Path to the GameObject.")]
            string path,
            [Description("Component class full name.")]
            string fullName)
        {
            return Task.Run(() => string.Empty);
            // return Execute(nameof(Add), commandData => commandData
            //     .SetOrAddParameter(nameof(path), path)
            //     .SetOrAddParameter(nameof(fullName), fullName));
        }
    }
}
#endif