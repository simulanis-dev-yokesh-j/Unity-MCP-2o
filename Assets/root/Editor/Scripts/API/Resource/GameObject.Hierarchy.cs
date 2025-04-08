using com.IvanMurzak.Unity.MCP.Common;
using UnityEngine;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    [ResourceType]
    public partial class Resource_GameObject
    {
        [Resource
        (
            Route = Consts.Route.GameObject_CurrentScene,
            MimeType = Consts.MimeType.TextPlain,
            Name = "GameObject.CurrentScene",
            Description = "Get gameObject(s) at the current scene. * - means to get children, ** - means to get all children recursively."
        )]
        public string CurrentScene(string path)
        {
            if (string.IsNullOrEmpty(path))
                return "[Error] Path to the GameObject is empty.";

            if (path == Consts.AllRecursive)
            {

            }
            if (path == Consts.All)
            {

            }

            return MainThread.Run(() =>
            {
                var go = GameObject.Find(path);
                if (go == null)
                    return $"[Error] GameObject '{path}' not found.";

                return go.name;
            });
        }
    }
}