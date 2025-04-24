#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.ComponentModel;
using System.Linq;
using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Editor.Utils;
using com.IvanMurzak.Unity.MCP.Utils;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    public partial class Tool_GameObject
    {
        [McpPluginTool
        (
            "GameObject_GetComponents",
            Title = "Get GameObject components",
            Description = @"Get components of the target GameObject. Returns property values of each component.
Returns list of all available components preview if no requested components found."
        )]
        public string GetComponents
        (
            [Description("The 'instanceID' array of the target components. Leave it empty if all components needed.")]
            int[] componentInstanceIDs,
            [Description("GameObject by 'instanceID' (int). Priority: 1. (Recommended)")]
            int instanceID = 0,
            [Description("GameObject by 'path'. Priority: 2.")]
            string? path = null,
            [Description("GameObject by 'name'. Priority: 3.")]
            string? name = null
        )
        => MainThread.Run(() =>
        {
            var go = GameObjectUtils.FindBy(instanceID, path, name, out var error);
            if (error != null)
                return error;

            var allComponents = go.GetComponents<UnityEngine.Component>();
            var components = allComponents
                .Where(c => componentInstanceIDs.Length == 0 || componentInstanceIDs.Contains(c.GetInstanceID()))
                .Select(c => Serializer.Component.BuildData(c))
                .ToList();

            if (components.Count == 0)
                return Error.NotFoundComponents(componentInstanceIDs, allComponents);

            var componentsJson = JsonUtils.Serialize(components);

            return @$"[Success] Found {components.Count} components in GameObject with 'instanceID'={go.GetInstanceID()}.
{go.Print()}

# Components:
{componentsJson}";
        });
    }
}