#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Utils;
using UnityEditor;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    public partial class Tool_Editor
    {
        [McpPluginTool
        (
            "Editor_StartPlaymode",
            Title = "Activate playmode in Unity Editor",
            Description = "Hits the play button in Unity Editor. Use it to start playmode and to start play the game."
        )]
        public string StartPlaymode()
        {
            return MainThread.Run(() =>
            {
                if (EditorApplication.isPlaying)
                    return $"[Success] Already in 'playmode'";

                EditorApplication.isPlaying = true;
                return $"[Success] Started 'playmode'";
            });
        }
    }
}