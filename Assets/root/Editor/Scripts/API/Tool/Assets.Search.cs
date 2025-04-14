#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.ComponentModel;
using System.Linq;
using com.IvanMurzak.Unity.MCP.Common;
using UnityEditor;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    [McpPluginToolType]
    public partial class Tool_Assets
    {
        [McpPluginTool
        (
            "Assets_Search",
            Title = "Search in the project assets",
            Description = "Search the asset database using the search filter string."
        )]
        public string Search
        (
            [Description("Searching filter. Could be empty.")]
            string? filter = null
        )
        => MainThread.Run(() =>
        {
            var assets = AssetDatabase.FindAssets(filter ?? string.Empty)
                .Select(AssetDatabase.GUIDToAssetPath)
                .ToList();

            return string.Join("\n", assets);
        });
    }
}