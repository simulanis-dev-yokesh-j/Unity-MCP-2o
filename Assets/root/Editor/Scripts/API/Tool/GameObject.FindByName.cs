#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.ComponentModel;
using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Editor.Utils;
using UnityEngine;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    public partial class Tool_GameObject
    {
        [McpPluginTool
        (
            "GameObject_FindByName",
            Title = "Find GameObject by name",
            Description = "Find GameObject by name in the active scene. Returns metadata about GameObject and it's children."
        )]
        public string FindByName
        (
            [Description("Name of the target GameObject.")]
            string name,
            [Description("Include children GameObjects in the result.")]
            bool includeChildren = true,
            [Description("Include children GameObjects recursively in the result. Ignored if 'includeChildren' is false.")]
            bool includeChildrenRecursively = false
        )
        {
            if (string.IsNullOrEmpty(name))
                return "[Error] GameObject name is empty.";

            return MainThread.Run(() =>
            {
                var go = GameObject.Find(name);
                if (go == null)
                    return $"[Error] GameObject with name '{name}' not found.";

                return go.ToMetadata(includeChildren, includeChildrenRecursively).ToString();
            });
        }
    }
}