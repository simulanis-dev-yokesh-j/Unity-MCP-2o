#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.ComponentModel;
using System.Linq;
using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Utils;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    public partial class Tool_GameObject
    {
        [McpPluginTool
        (
            "GameObject_Find",
            Title = "Find GameObject in opened Prefab or in a Scene",
            Description = @"Finds specific GameObject by provided information.
First it looks for the opened Prefab, if any Prefab is opened it looks only there ignoring a scene.
If no opened Prefab it looks into current active scene.
Returns GameObject infromation and its children.
Also, it returns Components preview just for the target GameObject."
        )]
        public string Find
        (
            [Description("Determines the depth of the hierarchy to include. 0 - means only the target GameObject. 1 - means to include one layer below.")]
            int includeChildrenDepth = 0,
            [Description("Find by 'instanceID' (int). Priority: 1. (Recommended)")]
            int instanceID = 0,
            [Description("Find by 'path'. Priority: 2.")]
            string? path = null,
            [Description("Find by 'name'. Priority: 3.")]
            string? name = null
        )
        {
            return MainThread.Run(() =>
            {
                // Find by 'instanceID' first, then by 'path', then by 'name'
                var go = GameObjectUtils.FindBy(instanceID, path, name, out var error);
                if (error != null)
                    return error;

                var components = go.GetComponents<UnityEngine.Component>();
                var componentsPreview = components
                    .Select(c => MCP.Utils.Serializer.Component.BuildDataLight(c))
                    .ToList();

                return @$"[Success] Found GameObject.
# Components preview:
{JsonUtils.Serialize(componentsPreview)}

# GameObject bounds:
{JsonUtils.Serialize(go.CalculateBounds())}

# GameObject information:
{go.ToMetadata(includeChildrenDepth).Print()}";
            });
        }
    }
}