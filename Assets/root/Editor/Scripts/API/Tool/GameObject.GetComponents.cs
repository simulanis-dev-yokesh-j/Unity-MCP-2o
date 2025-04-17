#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.ComponentModel;
using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Editor.Utils;
using com.IvanMurzak.Unity.MCP.Utils;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    public partial class Tool_GameObject
    {
        [McpPluginTool
        (
            "GameObject_GetComponents",
            Title = "Get GameObject components",
            Description = "Get all components of a GameObject by path. Returns property values of each component."
        )]
        public string GetComponents
        (
            [Description("Path to the GameObject.")]
            string path
        )
        {
            path = StringUtils.TrimPath(path);

            return MainThread.Run(() =>
            {
                if (string.IsNullOrEmpty(path))
                    return Error.GameObjectPathIsEmpty();

                var go = GameObjectUtils.FindByPath(path);
                if (go == null)
                    return Error.NotFoundGameObjectAtPath(path);

                return Serializer.GameObject.Serialize(go);
            });
        }
    }
}