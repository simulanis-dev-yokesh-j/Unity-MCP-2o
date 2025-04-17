#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.ComponentModel;
using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Editor.Utils;
using com.IvanMurzak.Unity.MCP.Utils;
using UnityEngine;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    public partial class Tool_GameObject
    {
        [McpPluginTool
        (
            "GameObject_FindByInstanceId",
            Title = "Find GameObject by InstanceId",
            Description = "Find GameObject in the active scene. Returns metadata about GameObject and its children."
        )]
        public string Find
        (
            [Description("Determines the depth of the hierarchy to include. 0 - means only the target GameObject. 1 - means to include one layer below.")]
            int includeChildrenDepth = 0,
            [Description("Find by 'instanceId'. Priority: 1. (Recommended)")]
            int? instanceId = null,
            [Description("Find by 'path'. Priority: 2.")]
            string? path = null,
            [Description("Find by 'name'. Priority: 3.")]
            string? name = null
        )
        {
            return MainThread.Run(() =>
            {
                // Find by 'instanceId' first, then by 'path', then by 'name'
                var go = GameObjectUtils.FindBy(instanceId, path, name, out var error);
                if (error != null)
                    return error;

                return go.ToMetadata(includeChildrenDepth).Print();
            });
        }
    }
}