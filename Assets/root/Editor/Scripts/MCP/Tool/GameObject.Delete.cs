using com.IvanMurzak.UnityMCP.Common.API;
using com.IvanMurzak.UnityMCP.Editor;
using UnityEditor;
using UnityEngine;

namespace com.IvanMurzak.UnityMCP.API.Editor
{
    public partial class Tool_GameObject
    {
        [Tool]
        public string Delete(string path) => MainThread.Run(() =>
        {
            var go = GameObject.Find(path);
            if (go == null)
                return $"[Error] GameObject '{path}' not found.";

            var scene = go.scene;
            GameObject.DestroyImmediate(go);
            return $"[Success] Deleted GameObject '{path}' from scene '{scene.name}'.";
        });
    }
}