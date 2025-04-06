using com.IvanMurzak.UnityMCP.Common;
using com.IvanMurzak.UnityMCP.Common.API;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Threading.Tasks;

namespace com.IvanMurzak.UnityMCP.API.Editor
{
    [McpServerToolType]
    public class Tool_GameObject : ServerTool
    {
        public override string Method => nameof(Create);

        [McpServerTool(Name = "CreateGameObject", Title = "Create GameObject")]
        [Description("Create a new GameObject in the current active scene.")]
        public Task<string> Create(
            [Description("Path to the parent GameObject.")]
            string path,
            [Description("Name of the new GameObject.")]
            string name)
        {
            return Execute(commandData => commandData
                .SetOrAddParameter(nameof(path), path)
                .SetOrAddParameter(nameof(name), name));
        }
    }
}