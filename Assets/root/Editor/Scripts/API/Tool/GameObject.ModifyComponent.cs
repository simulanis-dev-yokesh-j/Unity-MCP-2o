#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Nodes;
using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Common.Data.Unity;
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
            [Description("Json Object with required readonly 'instanceId' and 'type' fields. Any other field would be used for changing value in the target component. only required to modify properties and fields and with 'Type' field at the root. It should respect the original structure of the component.")]
            ComponentData data,
            [Description("GameObject by 'instanceId'. Priority: 1. (Recommended)")]
            int? instanceId = null,
            [Description("GameObject by 'path'. Priority: 2.")]
            string? path = null,
            [Description("GameObject by 'name'. Priority: 3.")]
            string? name = null
        )
        => MainThread.Run(() =>
        {
            if (string.IsNullOrEmpty(data?.type))
                return Error.InvalidComponentType(data?.type);

            var go = GameObjectUtils.FindBy(instanceId, path, name, out var error);
            if (error != null)
                return error;

            var allComponents = go.GetComponents<UnityEngine.Component>();
            var component = allComponents.FirstOrDefault(c => c.GetInstanceID() == data.instanceId);
            if (component == null)
                return Error.NotFoundComponent(data.instanceId, allComponents);

            var castedComponent = TypeUtils.CastTo(component, data.type, out error);
            if (error != null)
                return error;

            var type = Type.GetType(data.type);
            if (type == null)
                return Error.InvalidComponentType(data.type);

            if (!type.IsAssignableFrom(component.GetType()))
                return Error.TypeMismatch(data.type, component.GetType().FullName);

            if (data.properties == null || data.properties.Count == 0)
                return $"[Error] No properties provided to modify in component '{data.instanceId}' at GameObject.\n{go.Print()}";

            var changedProperties = new List<string>(data.properties.Count);

            // Modify component here (change properties, fields, etc.)
            foreach (var property in data.properties)
            {
                var propInfo = type.GetProperty(property.name);
                if (propInfo != null && propInfo.CanWrite)
                {
                    var propType = Type.GetType(property.type);
                    if (propType == null)
                        return Error.InvalidComponentPropertyType(property, propInfo);

                    var propValue = JsonUtils.Deserialize(property.json, propType);

                    propInfo.SetValue(component, propValue);

                    changedProperties.Add(property.name);
                    continue;
                }

                var fieldInfo = type.GetField(property.name);
                if (fieldInfo != null)
                {
                    var fieldType = Type.GetType(property.type);
                    if (fieldType == null)
                        return Error.InvalidComponentFieldType(property, fieldInfo);

                    var fieldValue = JsonUtils.Deserialize(property.json, fieldType);

                    fieldInfo.SetValue(component, fieldValue);

                    changedProperties.Add(property.name);
                }
            }

            var changedPropertiesString = string.Join(", ", changedProperties);
            return $"[Success] Modify component '{data.instanceId}' with properties [{changedPropertiesString}] at GameObject.\n{go.Print()}";
        });
    }
}