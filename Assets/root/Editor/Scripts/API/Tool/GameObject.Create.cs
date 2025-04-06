using com.IvanMurzak.Unity.MCP.Common;
using UnityEditor;
using UnityEngine;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    [ToolType]
    public partial class Tool_GameObject
    {
        [Tool]
        public string Create(string path, string name) => MainThread.Run(() =>
        {
            var targetParent = string.IsNullOrEmpty(path) ? null : GameObject.Find(path);
            if (targetParent == null && !string.IsNullOrEmpty(path))
                return $"[Error] Parent GameObject '{path}' not found.";

            var go = new GameObject(name);
            go.transform.position = new Vector3(0, 0, 0);
            go.transform.rotation = Quaternion.identity;
            go.transform.localScale = new Vector3(1, 1, 1);
            if (targetParent != null)
                go.transform.SetParent(targetParent.transform, false);

            EditorUtility.SetDirty(go);
            EditorApplication.RepaintHierarchyWindow();

            return $"[Success] Created GameObject '{name}' at path '{path}'.";
        });
    }
}