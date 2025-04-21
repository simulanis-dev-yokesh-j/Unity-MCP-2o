#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.ComponentModel;
using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Utils;
using UnityEditor;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    public partial class Tool_GameObject
    {
        [McpPluginTool
        (
            "GameObject_Select",
            Title = "Select GameObjects in opened scene",
            Description = @"Select GameObjects in opened scene by 'instanceId' (int) array."
        )]
        public string Select
        (
            [Description("The 'instanceId' array of the target GameObjects.")]
            int [] instanceIds
        )
        {
            return MainThread.Run(() =>
            {
                Selection.instanceIDs = instanceIds;

                return $"[Success] Selected {instanceIds.Length} GameObjects in opened scene by 'instanceId' (int) array.";
            });
        }
    }
}