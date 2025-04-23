#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Editor.Utils;
using com.IvanMurzak.Unity.MCP.Utils;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    public partial class Tool_GameObject
    {
        [McpPluginTool
        (
            "GameObject_DestroyComponents",
            Title = "Destroy Components from a GameObject",
            Description = "Destroy one or many components from target GameObject."
        )]
        public string DestroyComponents
        (
            [Description("The 'instanceId' array of the target components.")]
            int[] componentInstanceIds,
            [Description("GameObject by 'instanceId' (int). Priority: 1. (Recommended)")]
            int instanceId = 0,
            [Description("GameObject by 'path'. Priority: 2.")]
            string? path = null,
            [Description("GameObject by 'name'. Priority: 3.")]
            string? name = null
        )
        => MainThread.Run(() =>
        {
            var go = GameObjectUtils.FindBy(instanceId, path, name, out var error);
            if (error != null)
                return error;

            var destroyCounter = 0;
            var stringBuilder = new StringBuilder();

            var allComponents = go.GetComponents<UnityEngine.Component>();
            foreach (var component in allComponents)
            {
                var componentFullName = component.GetType().FullName;
                var componentInstanceId = component.GetInstanceID();
                if (componentInstanceIds.Contains(componentInstanceId))
                {
                    UnityEngine.Object.DestroyImmediate(component);
                    destroyCounter++;
                    stringBuilder.AppendLine($"[Success] Destroyed component instanceId='{componentInstanceId}', type='{componentFullName}'.");
                }
            }

            if (destroyCounter == 0)
                return Error.NotFoundComponents(componentInstanceIds, allComponents);

            return $"[Success] Destroyed {destroyCounter} components from GameObject.\n{stringBuilder.ToString()}";
        });
    }
}