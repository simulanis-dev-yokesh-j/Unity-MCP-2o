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
            Description = "Open a prefab. Use it for get started with prefab editing. There are two options to open prefab:\n" +
                          "1. Open prefab from asset using 'prefabAssetPath'.\n" +
                          "2. Open prefab from GameObject in loaded scene using 'instanceId' of the GameObject.\n" +
                          "   The GameObject should be connected to a prefab.\n\n" +
                          "Note: Please 'Close' the prefab after editing."
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
            if (string.IsNullOrEmpty(prefabAssetPath) && instanceId != 0)
            {
                // Open prefab from GameObject in loaded scene
                var go = GameObjectUtils.FindByInstanceId(instanceId);
                if (go == null)
                    return Tool_GameObject.Error.NotFoundGameObjectWithInstanceId(instanceId);

                prefabAssetPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(go);
            }

            if (string.IsNullOrEmpty(prefabAssetPath))
                return Error.PrefabPathIsEmpty();

            var prefab = AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(prefabAssetPath);
            if (prefab == null)
                return Error.NotFoundPrefabAtPath(prefabAssetPath);

            var success = AssetDatabase.OpenAsset(prefab);
            if (!success)
                return Error.NotFoundPrefabAtPath(prefabAssetPath);

            return @$"[Success] Prefab '{prefabAssetPath}' opened. Use 'Assets_Prefab_Close' to close it.
# Prefab information:
{prefab.ToMetadata().Print()}";
        });
    }
}

/*

var components = prefab.GetComponents<UnityEngine.Component>();
    var componentsPreview = components
        .Select(c => MCP.Utils.Serializer.Component.BuildDataLight(c))
        .ToList();

return @$"[Success] Found GameObject.
# Components preview:
{JsonUtils.Serialize(componentsPreview)}

# GameObject bounds:
{JsonUtils.Serialize(prefab.CalculateBounds())}

# GameObject information:
{prefab.ToMetadata(includeChildrenDepth).Print()}";

*/