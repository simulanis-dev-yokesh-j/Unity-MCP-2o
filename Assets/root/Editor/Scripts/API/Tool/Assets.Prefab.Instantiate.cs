#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.ComponentModel;
using System.Linq;
using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Editor.Utils;
using UnityEditor;
using UnityEngine;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    public partial class Tool_Assets_Prefab
    {
        [McpPluginTool
        (
            "Assets_Prefab_Instantiate",
            Title = "Instantiate prefab in the current active scene",
            Description = "Instantiates prefab in a scene."
        )]
        public string Instantiate
        (
            [Description("Prefab asset path.")]
            string prefabAssetPath,
            [Description("GameObject path in the current active scene.")]
            string gameObjectPath
        )
        => MainThread.Run(() =>
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabAssetPath);
            if (prefab == null)
                return Error.NotFoundPrefabAtPath(prefabAssetPath);

            var parentPath = StringUtils.Path_GetParentFolderPath(gameObjectPath);
            var name = StringUtils.Path_GetLastName(gameObjectPath);

            // If need to place the prefab in another GameObject
            if (parentPath != name)
            {
                var parentGo = GameObjectUtils.FindByPath(parentPath);
                if (parentGo == null)
                    return Tool_GameObject.Error.NotFoundGameObjectAtPath(parentPath);

                var go = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

                go.transform.SetParent(parentGo.transform, false);
                go.name = name;
            }
            else
            {
                var go = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                go.name = name;
            }

            return $"[Success] Prefab successfully instantiated at path: {gameObjectPath}";
        });
    }
}