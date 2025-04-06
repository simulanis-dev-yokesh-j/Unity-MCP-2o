using com.IvanMurzak.UnityMCP.Common.API;
using com.IvanMurzak.UnityMCP.Editor;
using UnityEditor;
using UnityEngine;

namespace com.IvanMurzak.UnityMCP.API.Editor
{
    public partial class Tool_GameObject
    {
        [Tool]
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