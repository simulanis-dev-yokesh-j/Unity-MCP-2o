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
            [Description("Position of the GameObject.")]
            Vector3? position = default,
            [Description("Rotation of the GameObject. Euler angles in degrees.")]
            Vector3? rotation = default,
            [Description("Scale of the GameObject.")]
            Vector3? scale = default
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

            var go = new GameObject(name);
            go.transform.position = position ?? Vector3.zero;
            go.transform.rotation = rotation == null
                ? Quaternion.identity
                : Quaternion.Euler(rotation.Value.x, rotation.Value.y, rotation.Value.z);
            go.transform.localScale = scale ?? Vector3.one;

            if (parentGo != null)
                go.transform.SetParent(parentGo.transform, false);

            EditorUtility.SetDirty(go);
            EditorApplication.RepaintHierarchyWindow();

            return $"[Success] Created GameObject.\n{go.Print()}";
        });
    }
}