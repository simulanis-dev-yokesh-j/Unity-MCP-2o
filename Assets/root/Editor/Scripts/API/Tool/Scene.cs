#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.Linq;
using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Editor.Utils;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    [McpPluginToolType]
    public partial class Tool_Scene
    {
        public static class Error
        {
            static string ScenesPrinted => string.Join("\n", SceneUtils.GetAllLoadedScenes().Select(scene => scene.name));

            public static string SceneNameIsEmpty()
                => $"[Error] Scene name is empty. Available scenes:\n{ScenesPrinted}";
            public static string NotFoundSceneWithName(string path)
                => $"[Error] Scene '{path}' not found. Available scenes:\n{ScenesPrinted}";
        }
    }
}