#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.ComponentModel;
using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Utils;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    public partial class Tool_GameObject
    {
        [McpPluginTool
        (
            "GameObject_Duplicate",
            Title = "Duplicate GameObjects in opened scene",
            Description = @"Duplicate GameObjects in opened scene by 'instanceID' (int) array."
        )]
        public string Duplicate
        (
            [Description("The 'instanceID' array of the target GameObjects.")]
            int [] instanceIDs
        )
        {
            return MainThread.Run(() =>
            {
                Selection.instanceIDs = instanceIDs;

                Unsupported.DuplicateGameObjectsUsingPasteboard();
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

                return @$"[Success] Duplicated {instanceIDs.Length} GameObjects in opened scene by 'instanceID' (int) array.
Duplicated instanceIDs:
{string.Join(", ", Selection.instanceIDs)}";
            });
        }
    }
}