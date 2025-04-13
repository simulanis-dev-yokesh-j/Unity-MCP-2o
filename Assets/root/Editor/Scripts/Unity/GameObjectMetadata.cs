#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace com.IvanMurzak.Unity.MCP.Editor
{
    public class GameObjectMetadata
    {
        public string path;
        public string tag;
        public bool activeSelf;
        public bool activeInHierarchy;
        public List<GameObjectMetadata> children = new();

        public override string ToString()
        {
            var sb = new StringBuilder();

            // Add table header
            sb.AppendLine("Path to root: " + path);
            sb.AppendLine("------------------------------------------------------------");
            sb.AppendLine("activeInHierarchy | activeSelf | tag       | name");
            sb.AppendLine("------------------|------------|-----------|----------------");

            // Add the current GameObject's metadata
            AppendMetadata(sb, this, 0);

            return sb.ToString();
        }

        private void AppendMetadata(StringBuilder sb, GameObjectMetadata metadata, int depth)
        {
            // Indent the path based on depth for better readability
            string indentedPath = new string(' ', depth * 2) + metadata.path;

            // Add the current GameObject's data
            sb.AppendLine($"{metadata.activeInHierarchy,-17} | {metadata.activeSelf,-10} | {metadata.tag,-9} | {indentedPath}");

            // Recursively add children
            foreach (var child in metadata.children)
            {
                AppendMetadata(sb, child, depth + 1);
            }
        }

        public static GameObjectMetadata FromGameObject(GameObject go, bool includeChildren = true, bool includeChildrenRecursively = false)
        {
            if (go == null)
                return null;

            if (includeChildrenRecursively && !includeChildren)
                throw new System.ArgumentException("includeChildrenRecursively cannot be true if includeChildren is false.");

            // Create metadata for the GameObject
            var metadata = new GameObjectMetadata
            {
                path = go.name,
                tag = go.tag,
                activeSelf = go.activeSelf,
                activeInHierarchy = go.activeInHierarchy
            };

            if (includeChildren)
            {
                metadata.children ??= new();
                foreach (Transform child in go.transform)
                {
                    var childMetadata = FromGameObject(child.gameObject, includeChildrenRecursively, includeChildrenRecursively);
                    metadata.children.Add(childMetadata);
                }
            }

            return metadata;
        }
    }
}