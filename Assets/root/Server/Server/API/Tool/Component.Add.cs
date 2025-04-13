using com.IvanMurzak.Unity.MCP.Server;
using ModelContextProtocol.Protocol.Types;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Threading.Tasks;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    public partial class Tool_Component
    {
        [McpServerTool
        (
            Name = "Component_Add",
            Title = "Add Component to a GameObject"
        )]
        [Description("Add a component to a GameObject.")]
        public Task<CallToolResponse> Add
        (
            [Description("Full name of the Component. It should include full namespace path and the class name.")]
            string componentName,
            [Description("Path to the GameObject (including the name of the GameObject).")]
            string gameObjectPath
        )
        {
            return ToolRouter.Call("Component_Add", arguments =>
            {
                arguments[nameof(componentName)] = componentName;
                arguments[nameof(gameObjectPath)] = gameObjectPath;
            });
        }
    }
}