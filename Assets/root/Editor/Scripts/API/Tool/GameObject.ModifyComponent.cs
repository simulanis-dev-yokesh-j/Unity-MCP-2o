#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Nodes;
using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Common.Data.Unity;
using com.IvanMurzak.Unity.MCP.Common.Data.Utils;
using com.IvanMurzak.Unity.MCP.Editor.Utils;
using com.IvanMurzak.Unity.MCP.Utils;
using UnityEngine;

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

            var type = TypeUtils.GetType(data.type);
            if (type == null)
                return Error.InvalidComponentType(data.type);

            if (!type.IsAssignableFrom(component.GetType()))
                return Error.TypeMismatch(data.type, component.GetType().FullName);

            // Validate properties
            if (data.properties == null || data.properties.Count == 0)
                return $"[Error] No properties provided to modify in component with 'instanceId'={data.instanceId} at GameObject with 'instanceId'={go.GetInstanceID()}.\n{go.Print()}";

            // Validate properties
            foreach (var property in data.properties)
            {
                if (string.IsNullOrEmpty(property.name))
                    return Error.ComponentPropertyNameIsEmpty();

                if (string.IsNullOrEmpty(property.type))
                    return Error.ComponentPropertyTypeIsEmpty();
            }

            var changedProperties = new List<string>(data.properties.Count);

            // Modify component here (change properties, fields, etc.)
            foreach (var property in data.properties)
            {
                var targetType = TypeUtils.GetType(property.type);
                if (targetType == null)
                {
                    if (McpPluginUnity.IsLogActive(LogLevel.Error))
                        Debug.LogError($"[Error] Type '{property.type}' not found. Can't modify property '{property.name}' in component '{data.instanceId}' at GameObject with 'instanceId'={go.GetInstanceID()}.\n{go.Print()}");
                }

                var propInfo = type.GetProperty(property.name);
                if (propInfo != null && propInfo.CanWrite)
                {
                    if (targetType == null)
                        return Error.InvalidComponentPropertyType(property, propInfo);

                    var propValue = property.value; // JsonUtils.Deserialize(property.json, targetType);

                    propInfo.SetValue(component, propValue);

                    changedProperties.Add(property.name);
                    continue;
                }

                var fieldInfo = type.GetField(property.name);
                if (fieldInfo != null)
                {
                    if (targetType == null)
                        return Error.InvalidComponentFieldType(property, fieldInfo);

                    var fieldValue = property.value; // JsonUtils.Deserialize(property.json, targetType);

                    fieldInfo.SetValue(component, fieldValue);

                    changedProperties.Add(property.name);
                    continue;
                }
            }

            var changedPropertiesString = string.Join(", ", changedProperties);
            return $"[Success] Modify component '{data.instanceId}' with properties [{changedPropertiesString}] at GameObject.\n{go.Print()}";
        });
    }
}