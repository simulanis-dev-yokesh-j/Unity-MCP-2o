#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.ComponentModel;
using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Utils;
using UnityEditor;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    public partial class Tool_Editor_Selection
    {
        [McpPluginTool
        (
            "Editor_Selection_Set",
            Title = "Set Selection in Unity Editor",
            Description = @"'UnityEditor.Selection'. Access to the selection in the editor.
Use it to select Assets or GameObjects in a scene. Set empty array to clear selection.
Selection.instanceIDs - The actual unfiltered selection from the Scene returned as instance ids.
Selection.activeInstanceID -  The 'instanceId' of the actual object selection. Includes Prefabs, non-modifiable objects."
        )]
        public string Set
        (
            [Description("The 'instanceId' array of the target GameObjects.")]
            int[]? instanceIds = null,
            [Description("The 'instanceId' of the target Object.")]
            int activeInstanceID = 0
        )
        {
            return MainThread.Run(() =>
            {
                Selection.instanceIDs = instanceIds ?? new int[0];
                Selection.activeInstanceID = activeInstanceID;

                // Selection.activeGameObject = EditorUtility.InstanceIDToObject(t);
                // Selection.activeObject = Selection.activeGameObject;
                // Selection.activeTransform = Selection.activeGameObject?.transform;

                return "[Success] " + SelectionPrint;
            });
        }
    }
}