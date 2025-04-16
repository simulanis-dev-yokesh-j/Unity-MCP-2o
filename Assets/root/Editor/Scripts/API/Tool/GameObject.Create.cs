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
            "GameObject_Create",
            Title = "Create a new GameObject",
            Description = @"Create a new GameObject at specific path.
if needed - provide proper 'position', 'rotation' and 'scale' to reduce amount of operations."
        )]
        public string Create
        (
            [Description("Path to the GameObject where it should be created. Can't be empty. Each intermediate GameObject should exist.")]
            string path,
            [Description("Transform position of the GameObject.")]
            Vector3? position = default,
            [Description("Transform rotation of the GameObject. Euler angles in degrees.")]
            Vector3? rotation = default,
            [Description("Transform scale of the GameObject.")]
            Vector3? scale = default,
            [Description("World or Local space of transform.")]
            bool isLocalSpace = false
        )
        => MainThread.Run(() =>
        {
            var parentGo = default(GameObject);
            if (StringUtils.Path_ParseParent(path, out var parentPath, out var name))
            {
                parentGo = GameObjectUtils.FindByPath(parentPath);
                if (parentGo == null)
                    return Error.NotFoundGameObjectAtPath(parentPath);
            }

            if (string.IsNullOrEmpty(name))
                return Error.GameObjectNameIsEmpty();

            var go = new GameObject(name);
            go.SetTransform(position, rotation, scale, isLocalSpace);

            if (parentGo != null)
                go.transform.SetParent(parentGo.transform, false);

            EditorUtility.SetDirty(go);
            EditorApplication.RepaintHierarchyWindow();

            return $"[Success] Created GameObject.\n{go.Print()}";
        });
    }
}