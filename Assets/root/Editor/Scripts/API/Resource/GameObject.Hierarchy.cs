using System.Linq;
using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Common.Data;
using com.IvanMurzak.Unity.MCP.Common.Utils;
using com.IvanMurzak.Unity.Runtime;
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
        public IResponseResourceContent[] CurrentScene(string uri, string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new System.Exception("[Error] Path to the GameObject is empty.");

            // if (path == Consts.AllRecursive)
            // {
            // }
            // if (path == Consts.All)
            // {
            // }

            return MainThread.Run(() =>
            {
                var go = GameObject.Find(path);
                //go.transform.rigidbody.transform
                if (go == null)
                    throw new System.Exception($"[Error] GameObject '{path}' not found.");

                var components = go.GetComponents<Component>();
                return ResponseResourceContent.CreateText(uri, JsonUtility.ToJson(components), Consts.MimeType.TextJson).MakeArray();
                // return ResponseResourceContent.CreateText(uri, ComponentSerializer.Serialize(components)).MakeArray();
                // return ResponseResourceContent.CreateText(uri, JsonUtils.Resource.ToJson(components)).MakeArray();
            });
        }

        public IResponseListResource[] CurrentSceneAll() => MainThread.Run(()
            => EditorSceneManager.GetActiveScene().GetRootGameObjects()
                .SelectMany(root => GameObjectUtils.GetAllRecursively(root))
                .Select(kvp => new ResponseListResource($"gameObject://currentScene/{kvp.Key}", kvp.Value.name, Consts.MimeType.TextJson))
                .ToArray());
    }
}