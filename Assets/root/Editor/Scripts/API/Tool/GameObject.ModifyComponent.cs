#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.ComponentModel;
using System.Linq;
using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Common.Data.Unity;
using com.IvanMurzak.Unity.MCP.Common.Data.Utils;
using com.IvanMurzak.Unity.MCP.Editor.Utils;
using com.IvanMurzak.Unity.MCP.Utils;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    public partial class Tool_GameObject
    {
        [McpPluginTool
        (
            "GameObject_ModifyComponent",
            Title = "Modify Component at GameObject",
            Description = "Modify existed component at GameObject."
        )]
        public string ModifyComponent
        (
            [Description(@"Json Object with required readonly 'instanceID' and 'type' fields.
Any other field would be used for changing value in the target component.
Only required to modify properties and fields and with 'Type' field at the root.
It should respect the original structure of the component.
Nested 'instanceID' fields and properties are references to UnityEngine.Object types.
The target reference instance could be located in project assets, in the scene or in the prefabs.")]
            ComponentData data,
            [Description("GameObject by 'instanceID' (int). Priority: 1. (Recommended)")]
            int instanceID = 0,
            [Description("GameObject by 'path'. Priority: 2.")]
            string? path = null,
            [Description("GameObject by 'name'. Priority: 3.")]
            string? name = null
        )
        => MainThread.Run(() =>
        {
            if (string.IsNullOrEmpty(data?.type))
                return Error.InvalidComponentType(data?.type);

            var type = TypeUtils.GetType(data.type);
            if (type == null)
                return Error.InvalidComponentType(data.type);

            var go = GameObjectUtils.FindBy(instanceID, path, name, out var error);
            if (error != null)
                return error;

            var allComponents = go.GetComponents<UnityEngine.Component>();
            var component = allComponents.FirstOrDefault(c => c.GetInstanceID() == data.instanceID);
            if (component == null)
                return Error.NotFoundComponent(data.instanceID, allComponents);

            var result = ReflectionUtils.Unity.ModifyComponent(component, data);

            return @$"Component with instanceID '{data.instanceID}' modification result:

{result.ToString()}

at GameObject.
{go.Print()}";

        });
    }
}