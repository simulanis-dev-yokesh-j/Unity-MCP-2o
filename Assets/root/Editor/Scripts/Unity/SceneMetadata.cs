#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.IvanMurzak.Unity.MCP.Editor.Utils;
using UnityEngine.SceneManagement;

namespace com.IvanMurzak.Unity.MCP.Editor
{
    public class SceneMetadata
    {
        public string path;
        public string name;
        public bool isDirty;
        public bool isLoaded;
        public List<GameObjectMetadata> rootGameObjects = new();

        public string Print()
        {
            var sb = new StringBuilder();

            // Add table header
            sb.AppendLine("# Scene Information");
            sb.AppendLine("name: " + name);
            sb.AppendLine("path: " + path);
            sb.AppendLine("isDirty: " + isDirty + ", isLoaded: " + isLoaded);
            sb.AppendLine();
            sb.AppendLine("# Scene Hierarchy of GameObjects");
            sb.AppendLine("-------------------------------------------------------------------------");
            sb.AppendLine("instanceId | activeInHierarchy | activeSelf | tag       | name");
            sb.AppendLine("-----------|-------------------|------------|-----------|----------------");

            // Add the current GameObject's metadata
            foreach (var rootGameObject in rootGameObjects)
                GameObjectMetadata.AppendMetadata(sb, rootGameObject, 0);

            return sb.ToString();
        }
        public static SceneMetadata FromScene(Scene scene, int includeChildrenDepth = 3)
        {
            if (!scene.IsValid())
                return null;

            return new SceneMetadata
            {
                path = scene.path,
                name = scene.name,
                isDirty = scene.isDirty,
                isLoaded = scene.isLoaded,
                rootGameObjects = GameObjectUtils.FindRootGameObjects(scene)
                    .Select(x => x.ToMetadata(includeChildrenDepth))
                    .ToList()
            };
        }
    }
}