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
            Name = "GameObject_Add_Component",
            Title = "Add Component to a GameObject"
        )]
        [Description("Add a component to a GameObject.")]
        public ValueTask<CallToolResponse> AddComponent
        (
            [Description("Full name of the Component. It should include full namespace path and the class name.")]
            string componentName,
            [Description("Path to the GameObject (including the name of the GameObject).")]
            string gameObjectPath
        )
        {
            return ToolRouter.Call("GameObject_Add_Component", arguments =>
            {
                arguments[nameof(componentName)] = componentName;
                arguments[nameof(gameObjectPath)] = gameObjectPath;
            });
        }
    }
}