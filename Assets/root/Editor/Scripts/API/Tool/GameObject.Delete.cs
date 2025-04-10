using com.IvanMurzak.Unity.MCP.Common;
using UnityEditor;
using UnityEngine;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    public partial class Tool_GameObject
    {
        [Tool(Name = "Delete", Description = "Delete a GameObject.")]
        public string Delete(string fullPath) => MainThread.Run(() =>
        {
            var go = GameObject.Find(fullPath);
            if (go == null)
                return $"[Error] GameObject '{fullPath}' not found.";

            var scene = go.scene;
            Object.DestroyImmediate(go);
            return $"[Success] Deleted GameObject '{fullPath}' from scene '{scene.name}'.";
        });
    }
}