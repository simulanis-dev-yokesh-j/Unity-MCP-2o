#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.IvanMurzak.Unity.MCP.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.IvanMurzak.Unity.MCP.Editor.Utils
{
    public static class GameObjectUtils
    {
        public static GameObject[] FindRootGameObjects(Scene? scene = null)
        {
            if (scene == null)
            {
                var rootGos = UnityEditor.SceneManagement.EditorSceneManager
                    .GetActiveScene()
                    .GetRootGameObjects();

                return rootGos;
            }
            else
            {
                return scene.Value.GetRootGameObjects();
            }
        }
        public static GameObject FindByPath(string path, GameObject? root = null)
        {
            path = StringUtils.TrimPath(path);

            if (string.IsNullOrEmpty(path))
                return null;

            // If root is null, search in the active scene's root GameObjects
            if (root == null)
            {
                var rootGos = FindRootGameObjects();
                var pathParts = path.Split('/');

                root = rootGos.FirstOrDefault(go => go.name == pathParts[0]);
                if (root == null)
                    return null;

                var currentGameObject = root;

                foreach (var part in pathParts.Skip(1))
                {
                    if (currentGameObject == null)
                        return null;

                    currentGameObject = FindChildByName(currentGameObject, part);
                }

                return currentGameObject;
            }
            else
            {
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
        }
        public static GameObject FindChildByName(this GameObject parent, string name)
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
        public static GameObject AddChild(this GameObject parent, string name)
        {
            if (parent == null || string.IsNullOrEmpty(name))
                return null;

            return parent.AddChild(new GameObject(name));
        }
        public static GameObject AddChild(this GameObject parent, GameObject child)
        {
            if (parent == null || child == null)
                return null;

            child.transform.SetParent(parent.transform, false);
            return child;
        }
        public static IEnumerable<KeyValuePair<string, GameObject>> GetAllRecursively(this GameObject root, string path = null)
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
        public static string GetPath(this GameObject go)
        {
            if (go == null)
                return null;

            var path = new StringBuilder(go.name);
            var currentTransform = go.transform.parent;

            while (currentTransform != null)
            {
                path.Insert(0, '/'); // Prepend '/' to the start
                path.Insert(0, currentTransform.name); // Prepend the name to the start
                currentTransform = currentTransform.parent;
            }

            return path.ToString();
        }
        public static string Print(this GameObject go) => go == null
            ? null
            : $"instanceID: {go.GetInstanceID()}, path: {go.GetPath()}";
        public static string Print(this IEnumerable<GameObject> gos)
        {
            var sb = new StringBuilder();
            foreach (var go in gos)
                sb.AppendLine(go.Print());

            return sb.ToString();
        }
        public static GameObjectMetadata ToMetadata(this GameObject go, int includeChildrenDepth = 3)
            => GameObjectMetadata.FromGameObject(go, includeChildrenDepth);
    }
}