#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.ComponentModel;
using System.Linq;
using com.IvanMurzak.Unity.MCP.Common;
using UnityEditor;

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
            // var prefabs = AssetDatabase.FindAssets("t:Prefab")
            //     .Select(AssetDatabase.GUIDToAssetPath)
            //     .ToList();

            // if (!string.IsNullOrEmpty(search))
            // {
            //     componentTypes = componentTypes
            //         .Where(typeName => typeName != null && typeName.Contains(search, StringComparison.OrdinalIgnoreCase))
            //         .ToList();
            // }

            return "[Error] Not yet implemented";
        });
    }
}