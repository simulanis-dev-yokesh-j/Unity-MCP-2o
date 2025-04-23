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
            [Description(@"Json Object with required readonly 'instanceId' and 'type' fields.
Any other field would be used for changing value in the target component.
Only required to modify properties and fields and with 'Type' field at the root.
It should respect the original structure of the component.
Nested 'instanceId' fields and properties are references to UnityEngine.Object types.
The target reference instance could be located in project assets, in the scene or in the prefabs.")]
            ComponentData data,
            [Description("GameObject by 'instanceId' (int). Priority: 1. (Recommended)")]
            int instanceId = 0,
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

            if (!type.IsAssignableFrom(component.GetType()))
                return Error.TypeMismatch(data.type, component.GetType().FullName);

            // Enable/Disable component if needed
            if (castedComponent is UnityEngine.Behaviour behaviour)
            {
                var setEnabled = data.isEnabled.ToBool();
                if (behaviour.enabled != setEnabled)
                    behaviour.enabled = setEnabled;
            }
            if (castedComponent is UnityEngine.Renderer renderer)
            {
                var setEnabled = data.isEnabled.ToBool();
                if (renderer.enabled != setEnabled)
                    renderer.enabled = setEnabled;
            }

            var changedFieldResults = new string[data.fields?.Count ?? 0];
            var changedPropertyResults = new string[data.properties?.Count ?? 0];

            if ((data.fields?.Count ?? 0) > 0)
            {
                // Modify fields
                for (var i = 0; i < data.fields.Count; i++)
                {
                    var field = data.fields[i];

                    if (string.IsNullOrEmpty(field.name))
                    {
                        changedFieldResults[i] = Error.ComponentFieldNameIsEmpty();
                        continue;
                    }
                    if (string.IsNullOrEmpty(field.type))
                    {
                        changedFieldResults[i] = Error.ComponentFieldTypeIsEmpty();
                        continue;
                    }

                    var targetType = TypeUtils.GetType(field.type);
                    if (targetType == null)
                    {
                        if (McpPluginUnity.IsLogActive(LogLevel.Error))
                            Debug.LogError($"[Error] Type '{field.type}' not found. Can't modify field '{field.name}'.", go);
                    }

                    var fieldInfo = type.GetField(field.name);
                    if (fieldInfo == null)
                    {
                        changedFieldResults[i] = $"[Error] Field '{field.name}' not found. Can't modify field '{field.name}'.";
                        if (McpPluginUnity.IsLogActive(LogLevel.Warning))
                            Debug.LogWarning(changedFieldResults[i], go);
                        continue;
                    }
                    if (targetType == null)
                    {
                        changedFieldResults[i] = Error.InvalidComponentFieldType(field, fieldInfo);
                        continue;
                    }

                    // The `targetType` is a UnityEngine.Object type, so it should be handled differently.
                    if (typeof(UnityEngine.Object).IsAssignableFrom(targetType))
                    {
                        var referenceInstanceId = JsonUtils.Deserialize<InstanceId>(field.valueJsonElement.Value).instanceId;
                        if (referenceInstanceId == 0)
                        {
                            changedFieldResults[i] = Error.InvalidInstanceId(targetType, field.name);
                            continue;
                        }

                        // Find the object by instanceId
                        var referenceObject = UnityEditor.EditorUtility.InstanceIDToObject(referenceInstanceId);

                        if (typeof(Sprite).IsAssignableFrom(targetType) && referenceObject is Texture2D texture)
                        {
                            // Try to find the first Sprite sub-asset in the Texture2D asset
                            var assetPath = UnityEditor.AssetDatabase.GetAssetPath(texture);
                            var sprite = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(assetPath)
                                .OfType<Sprite>()
                                .FirstOrDefault();
                            referenceObject = sprite;
                            fieldInfo.SetValue(component, referenceObject);
                            changedFieldResults[i] = $"[Success] Field '{field.name}' modified to '{referenceObject}'.";
                        }
                        else
                        {
                            // Cast the object to the target type
                            var castedObject = TypeUtils.CastTo(referenceObject, targetType, out error);
                            if (error != null)
                            {
                                changedFieldResults[i] = error;
                                continue;
                            }

                            fieldInfo.SetValue(component, castedObject);
                            changedFieldResults[i] = $"[Success] Field '{field.name}' modified to '{castedObject}'.";
                        }
                    }
                    else
                    {
                        try
                        {
                            var fieldValue = JsonUtils.Deserialize(field.valueJsonElement.Value.GetRawText(), targetType);
                            fieldInfo.SetValue(component, fieldValue);
                            changedFieldResults[i] = $"[Success] Field '{field.name}' modified to '{fieldValue}'.";
                        }
                        catch (System.Exception ex)
                        {
                            changedFieldResults[i] = $"[Error] Field '{field.name}' modification failed: {ex.Message}";
                            if (McpPluginUnity.IsLogActive(LogLevel.Error))
                                Debug.LogError(changedFieldResults[i], go);
                            continue;
                        }
                    }
                }
            }

            if ((data.properties?.Count ?? 0) > 0)
            {
                // Modify properties
                for (var i = 0; i < data.properties.Count; i++)
                {
                    var property = data.properties[i];

                    if (string.IsNullOrEmpty(property.name))
                    {
                        changedPropertyResults[i] = Error.ComponentPropertyNameIsEmpty();
                        continue;
                    }
                    if (string.IsNullOrEmpty(property.type))
                    {
                        changedPropertyResults[i] = Error.ComponentPropertyTypeIsEmpty();
                        continue;
                    }

                    var targetType = TypeUtils.GetType(property.type);
                    if (targetType == null)
                    {
                        if (McpPluginUnity.IsLogActive(LogLevel.Warning))
                            Debug.LogWarning($"[Error] Type '{property.type}' not found. Can't modify property '{property.name}'.", go);
                    }

                    var propInfo = type.GetProperty(property.name);
                    if (propInfo == null)
                    {
                        changedPropertyResults[i] = $"[Error] Property '{property.name}' not found. Can't modify property '{property.name}'.";
                        if (McpPluginUnity.IsLogActive(LogLevel.Warning))
                            Debug.LogWarning(changedPropertyResults[i], go);
                        continue;
                    }
                    if (!propInfo.CanWrite)
                    {
                        changedPropertyResults[i] = $"[Error] Property '{property.name}' is not writable. Can't modify property '{property.name}'.";
                        if (McpPluginUnity.IsLogActive(LogLevel.Warning))
                            Debug.LogWarning(changedPropertyResults[i], go);
                        continue;
                    }

                    if (targetType == null)
                    {
                        changedPropertyResults[i] = Error.InvalidComponentPropertyType(property, propInfo);
                        continue;
                    }

                    // The `targetType` is a UnityEngine.Object type, so it should be handled differently.
                    if (typeof(UnityEngine.Object).IsAssignableFrom(targetType))
                    {
                        var referenceInstanceId = JsonUtils.Deserialize<InstanceId>(property.valueJsonElement.Value).instanceId;
                        if (referenceInstanceId == 0)
                        {
                            changedPropertyResults[i] = Error.InvalidInstanceId(targetType, property.name);
                            continue;
                        }

                        // Find the object by instanceId
                        var referenceObject = UnityEditor.EditorUtility.InstanceIDToObject(referenceInstanceId);

                        if (typeof(Sprite).IsAssignableFrom(targetType) && referenceObject is Texture2D texture)
                        {
                            // Try to find the first Sprite sub-asset in the Texture2D asset
                            var assetPath = UnityEditor.AssetDatabase.GetAssetPath(texture);
                            var sprite = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(assetPath)
                                .OfType<Sprite>()
                                .FirstOrDefault();
                            referenceObject = sprite;
                            propInfo.SetValue(component, referenceObject);
                            changedPropertyResults[i] = $"[Success] Property '{property.name}' modified to '{referenceObject}'.";
                        }
                        else
                        {
                            // Cast the object to the target type
                            var castedObject = TypeUtils.CastTo(referenceObject, targetType, out error);
                            if (error != null)
                            {
                                changedPropertyResults[i] = error;
                                continue;
                            }

                            propInfo.SetValue(component, castedObject);
                            changedPropertyResults[i] = $"[Success] Property '{property.name}' modified to '{castedObject}'.";
                        }
                    }
                    else
                    {
                        try
                        {
                            var propValue = JsonUtils.Deserialize(property.valueJsonElement.Value.GetRawText(), targetType);
                            propInfo.SetValue(component, propValue);
                            changedPropertyResults[i] = $"[Success] Property '{property.name}' modified to '{propValue}'.";
                        }
                        catch (System.Exception ex)
                        {
                            changedPropertyResults[i] = $"[Error] Property '{property.name}' modification failed: {ex.Message}";
                            if (McpPluginUnity.IsLogActive(LogLevel.Error))
                                Debug.LogError(changedPropertyResults[i], go);
                            continue;
                        }
                    }
                }
            }

            return @$"Component modification result in component '{data.instanceId}':

Field modification results:
{string.Join("\n", changedFieldResults)}
----------------------------

Property modification results:
{string.Join("\n", changedPropertyResults)}
----------------------------

at GameObject.
{go.Print()}";

        });
    }
}