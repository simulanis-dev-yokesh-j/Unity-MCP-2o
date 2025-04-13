#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.ComponentModel;
using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Editor.Utils;
using UnityEditor;
using UnityEngine;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    public partial class Tool_GameObject
    {
        [McpPluginTool
        (
            "GameObject_Destroy",
            Title = "Destroy GameObject",
            Description = "Destroy a GameObject."
        )]
        public string Delete
        (
            [Description("Path to the GameObject to destroy.")]
            string fullPath
        )
        => MainThread.Run(() =>
        {
            var go = GameObjectUtils.FindByPath(fullPath);
            if (go == null)
                return $"[Error] GameObject '{fullPath}' not found.";

            var scene = go.scene;
            Object.DestroyImmediate(go);
            return $"[Success] Destroy GameObject '{fullPath}' from scene '{scene.name}'.";
        });
    }
}