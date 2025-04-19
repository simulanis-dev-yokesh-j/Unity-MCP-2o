#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

            // Validate - at least one field or property should be provided
            if (data.fields == null || data.fields.Count == 0 ||
                data.properties == null || data.properties.Count == 0)
                return $"[Error] Neither fields and properties provided to modify in component with 'instanceId'={data.instanceId} at GameObject with 'instanceId'={go.GetInstanceID()}.\n{go.Print()}";

            // Validate fields
            foreach (var field in data.fields)
            {
                if (string.IsNullOrEmpty(field.name))
                    return Error.ComponentFieldNameIsEmpty();

                if (string.IsNullOrEmpty(field.type))
                    return Error.ComponentFieldTypeIsEmpty();
            }

            // Validate properties
            foreach (var property in data.properties)
            {
                if (string.IsNullOrEmpty(property.name))
                    return Error.ComponentPropertyNameIsEmpty();

                if (string.IsNullOrEmpty(property.type))
                    return Error.ComponentPropertyTypeIsEmpty();
            }

            var changedFields = new List<string>(data.fields.Count);
            var changedProperties = new List<string>(data.properties.Count);

            // Modify fields
            foreach (var field in data.fields)
            {
                var targetType = TypeUtils.GetType(field.type);
                if (targetType == null)
                {
                    if (McpPluginUnity.IsLogActive(LogLevel.Error))
                        Debug.LogError($"[Error] Type '{field.type}' not found. Can't modify field '{field.name}' in component '{data.instanceId}' at GameObject with 'instanceId'={go.GetInstanceID()}.", go);
                }

                var fieldInfo = type.GetField(field.name);
                if (fieldInfo == null)
                {
                    if (McpPluginUnity.IsLogActive(LogLevel.Warning))
                        Debug.LogWarning($"[Error] Field '{field.name}' not found. Can't modify field '{field.name}' in component '{data.instanceId}' at GameObject with 'instanceId'={go.GetInstanceID()}.", go);
                    continue;
                }
                if (targetType == null)
                    return Error.InvalidComponentFieldType(field, fieldInfo);

                var fieldValue = JsonUtility.FromJson(field.valueJsonElement.Value.GetRawText(), targetType);

                fieldInfo.SetValue(component, fieldValue);

                changedFields.Add(field.name);
            }

            // Modify properties
            foreach (var property in data.properties)
            {
                var targetType = TypeUtils.GetType(property.type);
                if (targetType == null)
                {
                    if (McpPluginUnity.IsLogActive(LogLevel.Warning))
                        Debug.LogWarning($"[Error] Type '{property.type}' not found. Can't modify property '{property.name}' in component '{data.instanceId}' at GameObject with 'instanceId'={go.GetInstanceID()}.", go);
                }

                var propInfo = type.GetProperty(property.name);
                if (propInfo == null)
                {
                    if (McpPluginUnity.IsLogActive(LogLevel.Warning))
                        Debug.LogWarning($"[Error] Property '{property.name}' not found. Can't modify property '{property.name}' in component '{data.instanceId}' at GameObject with 'instanceId'={go.GetInstanceID()}.", go);
                    continue;
                }
                if (propInfo.CanWrite)
                {
                    if (McpPluginUnity.IsLogActive(LogLevel.Warning))
                        Debug.LogWarning($"[Warning] Property '{property.name}' is not writable. Can't modify property '{property.name}' in component '{data.instanceId}' at GameObject with 'instanceId'={go.GetInstanceID()}.", go);
                    continue;
                }

                if (targetType == null)
                    return Error.InvalidComponentPropertyType(property, propInfo);

                var propValue = JsonUtility.FromJson(property.valueJsonElement.Value.GetRawText(), targetType);
                propInfo.SetValue(component, propValue);

                changedProperties.Add(property.name);
            }

            var changedFieldsString = string.Join(", ", changedFields);
            var changedPropertiesString = string.Join(", ", changedProperties);

            return @$"[Success] Modify component '{data.instanceId}' with:
Modified fields:
[{changedFieldsString}]
----------------------------
Modified Properties:
[{changedPropertiesString}]
----------------------------
at GameObject.\n{go.Print()}";
        });
    }
}