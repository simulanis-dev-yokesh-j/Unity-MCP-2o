#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.IvanMurzak.Unity.MCP.Editor.Utils
{
    public static class GameObjectUtils
    {
        public static GameObject FindByPath(string path, GameObject? root = null)
        {
            if (string.IsNullOrEmpty(path))
                return null;

            if (path.StartsWith("/") ||
                path.StartsWith("./"))
                path = path[1..];

            // If root is null, search in the active scene's root GameObjects
            if (root == null)
            {
                path = path.TrimStart('/');
                return UnityEditor.SceneManagement.EditorSceneManager
                    .GetActiveScene()
                    .GetRootGameObjects()
                    .FirstOrDefault(go => FindByPath(path, go) != null);
            }

            var pathParts = path.Split('/');
            var currentGameObject = root;

            foreach (var part in pathParts)
            {
                if (currentGameObject == null)
                    return null;

                currentGameObject = FindChildByName(currentGameObject, part);
            }

            return currentGameObject;
        }
        public static GameObject FindChildByName(GameObject parent, string name)
        {
            if (parent == null || string.IsNullOrEmpty(name))
                return null;

            foreach (Transform child in parent.transform)
            {
                if (child.name == name)
                    return child.gameObject;
            }

            return null;
        }
        public static IEnumerable<KeyValuePair<string, GameObject>> GetAllRecursively(GameObject root, string path = null)
        {
            var currentPath = string.IsNullOrEmpty(path)
                ? root.name
                : $"{path}/{root.name}";

            yield return new(currentPath, root);

            foreach (Transform child in root.transform)
            {
                foreach (var childContent in GetAllRecursively(child.gameObject, currentPath))
                {
                    yield return childContent;
                }
            }
        }
    }
}