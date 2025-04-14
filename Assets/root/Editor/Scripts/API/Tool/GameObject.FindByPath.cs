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
            [Description("Determines the depth of the hierarchy to include.")]
            int includeChildrenDepth = 3
        )
        {
            path = StringUtils.TrimPath(path);

            return MainThread.Run(() =>
            {
                var go = GameObjectUtils.FindByPath(path);
                if (go == null)
                    return Error.NotFoundGameObjectAtPath(path);

                return go.ToMetadata(includeChildrenDepth).Print();
            });
        }
    }
}