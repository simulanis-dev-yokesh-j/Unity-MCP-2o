#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.ComponentModel;
using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Editor.Utils;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    public partial class Tool_GameObject
    {
        [McpPluginTool
        (
            "GameObject_FindByPath",
            Title = "Find GameObject by path",
            Description = "Find GameObject tree by the path to the root GameObject. Returns metadata about each GameObject."
        )]
        public string FindByPath
        (
            [Description("Path to the GameObject.")]
            string path,
            [Description("Include children GameObjects in the result.")]
            bool includeChildren = true,
            [Description("Include children GameObjects recursively in the result. Ignored if 'includeChildren' is false.")]
            bool includeChildrenRecursively = false
        )
        {
            path = StringUtils.TrimPath(path);

            return MainThread.Run(() =>
            {
                var go = GameObjectUtils.FindByPath(path);
                if (go == null)
                    return $"[Error] GameObject '{path}' not found.";

                return go.ToMetadata(includeChildren, includeChildrenRecursively).ToString();
            });
        }
    }
}