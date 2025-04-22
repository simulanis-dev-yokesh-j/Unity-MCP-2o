#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Editor.Utils;
using com.IvanMurzak.Unity.MCP.Utils;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    public partial class Tool_Scene
    {
        [McpPluginTool
        (
            "Scene_Unload",
            Title = "Unload scene",
            Description = "Unload loaded scene."
        )]
        public Task<string> Unload
        (
            [Description("Name of the loaded scene.")]
            string name
        )
        => MainThread.Run(async () =>
        {
            if (string.IsNullOrEmpty(name))
                return Error.ScenePathIsEmpty();

            var scene = SceneUtils.GetAllLoadedScenes()
                .FirstOrDefault(scene => scene.name == name);

            if (!scene.IsValid())
                return Error.NotFoundSceneWithName(name);

            var asyncOperation = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene);

            while (!asyncOperation.isDone)
                await Task.Yield();

            if (asyncOperation.isDone == false)
                return $"[Error] Failed to unload scene '{name}'.\n{LoadedScenes}";

            return $"[Success] Scene '{name}' unloaded.\n{LoadedScenes}";
        });
    }
}