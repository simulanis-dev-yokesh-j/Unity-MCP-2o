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
            "Assets_Prefabs_GetAll",
            Title = "Get list of all prefabs",
            Description = "Returns the list of all available prefabs in the project."
        )]
        public string GetAll
        (
            [Description("Substring for searching prefabs. Could be empty.")]
            string? search = null
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

            return string.Empty;
        });
    }
}