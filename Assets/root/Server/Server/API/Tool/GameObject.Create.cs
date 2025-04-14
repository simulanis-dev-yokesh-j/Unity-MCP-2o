using com.IvanMurzak.Unity.MCP.Server.API.Data;
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
            Name = "GameObject_Create",
            Title = "Create GameObject"
        )]
        [Description("Create a new GameObject in the current active scene.")]
        public Task<CallToolResponse> Create
        (
            [Description("Path to the parent GameObject.")]
            string path,
            [Description("Name of the new GameObject.")]
            string name,
            [Description("Position of the GameObject.")]
            Vector3? position = null,
            [Description("Rotation of the GameObject. Euler angles in degrees.")]
            Vector3? rotation = null,
            [Description("Scale of the GameObject.")]
            Vector3? scale = null
        )
        {
            return ToolRouter.Call("GameObject_Create", arguments =>
            {
                arguments[nameof(path)] = path;
                arguments[nameof(name)] = name;

                if (position != null)
                    arguments[nameof(position)] = position;

                if (rotation != null)
                    arguments[nameof(rotation)] = rotation;

                if (scale != null)
                    arguments[nameof(scale)] = scale;
            });
        }
    }
}