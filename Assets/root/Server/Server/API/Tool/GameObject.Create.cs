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
        public ValueTask<CallToolResponse> Create
        (
            [Description("Path to the GameObject where it should be created. Can't be empty. Each intermediate GameObject should exist.")]
            string path,
            [Description("Position of the GameObject.")]
            Vector3? position = default,
            [Description("Rotation of the GameObject. Euler angles in degrees.")]
            Vector3? rotation = default,
            [Description("Scale of the GameObject.")]
            Vector3? scale = default
        )
        {
            return ToolRouter.Call("GameObject_Create", arguments =>
            {
                arguments[nameof(path)] = path;

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