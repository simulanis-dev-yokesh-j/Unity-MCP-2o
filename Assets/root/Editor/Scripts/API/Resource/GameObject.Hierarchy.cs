using com.IvanMurzak.Unity.MCP.Common;
using UnityEngine;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    [ResourceType]
    public partial class Resource_GameObject
    {
        [Resource
        (
            Routing = Consts.Route.GameObject_CurrentScene,
            Name = "GameObject.CurrentScene",
            Description = "Get the name of the current scene.",
            MimeType = Consts.MimeType.TextPlain
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