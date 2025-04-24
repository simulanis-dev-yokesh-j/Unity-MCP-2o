#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.ComponentModel;
using System.Linq;
using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Editor.Utils;
using com.IvanMurzak.Unity.MCP.Utils;
using UnityEditor;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    public partial class Tool_Assets_Prefab
    {
        [McpPluginTool
        (
            "Assets_Prefab_Open",
            Title = "Open prefab",
            Description = "Open a prefab. There are two options to open prefab:\n" +
                          "1. Open prefab from asset using 'prefabAssetPath'.\n" +
                          "2. Open prefab from GameObject in loaded scene using 'instanceId' of the GameObject.\n" +
                          "   The GameObject should be connected to a prefab.\n\n" +
                          "Note: Please 'Close' the prefab later to exit prefab editing mode."
        )]
        public string Open
        (
            [Description("'instanceId' of GameObject in a scene.")]
            int instanceId = 0,
            [Description("Prefab asset path. Should be in the format 'Assets/Path/To/Prefab.prefab'.")]
            string? prefabAssetPath = null
        )
        => MainThread.Run(() =>
        {
            var prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
            // if (prefabStage != null)
            //     return Error.PrefabStageIsAlreadyOpened();

            if (string.IsNullOrEmpty(prefabAssetPath) && instanceId != 0)
            {
                // Find prefab from GameObject in loaded scene
                var go = GameObjectUtils.FindByInstanceId(instanceId);
                if (go == null)
                    return Tool_GameObject.Error.NotFoundGameObjectWithInstanceId(instanceId);

                prefabAssetPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(go);
            }

            if (string.IsNullOrEmpty(prefabAssetPath))
                return Error.PrefabPathIsEmpty();

            var goInstance = instanceId != 0
                ? GameObjectUtils.FindByInstanceId(instanceId)
                : null;

            prefabStage = goInstance != null
                ? UnityEditor.SceneManagement.PrefabStageUtility.OpenPrefab(prefabAssetPath, goInstance)
                : UnityEditor.SceneManagement.PrefabStageUtility.OpenPrefab(prefabAssetPath);

            if (prefabStage == null)
                return Error.PrefabStageIsNotOpened();

            return @$"[Success] Prefab '{prefabStage.assetPath}' opened. Use 'Assets_Prefab_Close' to close it.
# Prefab information:
{prefabStage.prefabContentsRoot.ToMetadata().Print()}";
        });
    }
}