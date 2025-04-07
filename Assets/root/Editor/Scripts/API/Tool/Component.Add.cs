using System;
using com.IvanMurzak.Unity.MCP.Common;
using UnityEditor;
using UnityEngine;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    [ToolType]
    public partial class Tool_Component
    {
        [Tool]
        public string Add(string path, string fullName)
        {
            if (string.IsNullOrEmpty(path))
                return "[Error] Path to the GameObject is empty.";

            if (string.IsNullOrEmpty(fullName))
                return "[Error] Full name of the component is empty.";

            var type = Type.GetType(fullName);
            if (type == null)
                return $"[Error] Component type '{fullName}' not found.";

            if (!typeof(Component).IsAssignableFrom(type))
                return $"[Error] Type '{fullName}' is not a valid Unity Component.";

            return MainThread.Run(() =>
            {
                var go = GameObject.Find(path);
                if (go == null)
                    return $"[Error] GameObject '{path}' not found.";

                go.AddComponent(type);

                EditorUtility.SetDirty(go);
                EditorApplication.RepaintHierarchyWindow();

                return $"[Success] Added `{fullName}` component to GameObject at path '{path}'.";
            });
        }
    }
}