using System.Collections.Generic;
using UnityEngine;

namespace com.IvanMurzak.Unity.MCP.Common.Utils
{
    public static class GameObjectUtils
    {
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