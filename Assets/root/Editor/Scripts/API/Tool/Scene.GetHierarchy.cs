#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.ComponentModel;
using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Editor.Utils;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    public partial class Tool_Scene
    {
        [McpPluginTool
        (
            "Scene_GetHierarchyRoot",
            Title = "Get Scene Hierarchy",
            Description = "This tool retrieves the list of root GameObjects in the specified scene."
        )]
        public string GetHierarchyRoot
        (
            [Description("Name of the loaded scene. If empty, the active scene will be used.")]
            string? loadedSceneName = null
        )
        => MainThread.Run(() =>
        {
            var scene = string.IsNullOrEmpty(loadedSceneName)
                ? UnityEngine.SceneManagement.SceneManager.GetActiveScene()
                : UnityEngine.SceneManagement.SceneManager.GetSceneByName(loadedSceneName);

            if (scene.IsValid())
                return Error.NotFoundSceneWithName(loadedSceneName);

            return GameObjectUtils.FindRootGameObjects(scene).Print();
        });
    }
}