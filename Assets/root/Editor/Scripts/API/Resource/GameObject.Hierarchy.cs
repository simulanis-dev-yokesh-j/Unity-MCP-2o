using System.Linq;
using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Common.Data;
using com.IvanMurzak.Unity.MCP.Common.Utils;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    [ResourceType]
    public partial class Resource_GameObject
    {
        [Resource
        (
            Route = "gameObject://currentScene/{path}",
            MimeType = Consts.MimeType.TextJson,
            ListResources = nameof(CurrentSceneAll),
            Name = "GameObject.CurrentScene",
            Description = "Get gameObject's components and the values of each explicit property."
        )]
        public string CurrentScene(string path)
        {
            if (string.IsNullOrEmpty(path))
                return "[Error] Path to the GameObject is empty.";

            // if (path == Consts.AllRecursive)
            // {
            // }
            // if (path == Consts.All)
            // {
            // }

            return MainThread.Run(() =>
            {
                var go = GameObject.Find(path);
                if (go == null)
                    return $"[Error] GameObject '{path}' not found.";

                var components = go.GetComponents<Component>();
                return JsonUtils.Resource.ToJson(components);
            });
        }

        public ResponseListResource[] CurrentSceneAll() => MainThread.Run(()
            => EditorSceneManager.GetActiveScene().GetRootGameObjects()
                .SelectMany(root => GameObjectUtils.GetAllRecursively(root))
                .Select(kvp => new ResponseListResource($"gameObject://currentScene/{kvp.Key}", kvp.Value.name, Consts.MimeType.TextJson))
                .ToArray());
    }
}