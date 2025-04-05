using System;
using System.ComponentModel;
using ModelContextProtocol.Server;
using UnityEngine;

namespace com.IvanMurzak.UnityMCP.Editor.Tools
{
    [McpServerToolType]
    public static class GameObject_Create
    {
        [McpServerTool(Name = "CreateGameObject", Title = "Create GameObject")]
        [Description("Create a new GameObject in the current active scene.")]
        public static string Create(
            [Description("Path to the parent GameObject.")]
            string path,
            [Description("Name of the new GameObject.")]
            string name)
        {
            try
            {
                var go = new GameObject(name);
                go.transform.position = new Vector3(0, 0, 0);
                go.transform.rotation = Quaternion.identity;
                go.transform.localScale = new Vector3(1, 1, 1);
                go.transform.SetParent(GameObject.Find(path).transform, false);
                return go.GetInstanceID().ToString();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to create GameObject: {ex.Message}");
                return ex.ToString();
            }
        }
    }
}