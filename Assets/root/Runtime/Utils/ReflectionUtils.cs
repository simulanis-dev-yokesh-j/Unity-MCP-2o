using System;
using System.Linq;
using System.Reflection;
using System.Text;
using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Common.Data.Utils;
using UnityEngine;

namespace com.IvanMurzak.Unity.MCP.Utils
{
    public static partial class ReflectionUtils
    {
        public static StringBuilder Modify(ref object obj, SerializedMember data, StringBuilder stringBuilder = null, int depth = 0,
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
        {
            stringBuilder ??= new StringBuilder();

            if (string.IsNullOrEmpty(data?.type))
                return stringBuilder.AppendLine(new string(' ', depth) + Error.DataTypeIsEmpty());

            var type = TypeUtils.GetType(data.type);
            if (type == null)
                return stringBuilder.AppendLine(new string(' ', depth) + Error.NotFoundType(data.type));

            if (obj == null)
                return stringBuilder.AppendLine(new string(' ', depth) + Error.TargetObjectIsNull());

            var castedObj = TypeUtils.CastTo(obj, data.type, out var error);
            if (error != null)
                return stringBuilder.AppendLine(new string(' ', depth) + error);

            if (!type.IsAssignableFrom(castedObj.GetType()))
                return stringBuilder.AppendLine(new string(' ', depth) + Error.TypeMismatch(data.type, obj.GetType().FullName));

            // Modify fields
            if ((data.fields?.Count ?? 0) > 0)
                for (var i = 0; i < data.fields.Count; i++)
                    ModifyField(ref castedObj, data.fields[i], stringBuilder, depth + 1, bindingFlags);

            // Modify properties
            if ((data.properties?.Count ?? 0) > 0)
                for (var i = 0; i < data.properties.Count; i++)
                    ModifyProperty(ref castedObj, data.properties[i], stringBuilder, depth + 1, bindingFlags);

            return stringBuilder;
        }

        public static StringBuilder ModifyField(ref object obj, SerializedMember fieldValue, StringBuilder stringBuilder = null, int depth = 0,
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
        {
            stringBuilder ??= new StringBuilder();

            if (string.IsNullOrEmpty(fieldValue.name))
                return stringBuilder.AppendLine(new string(' ', depth) + Error.ComponentFieldNameIsEmpty());

            if (string.IsNullOrEmpty(fieldValue.type))
                return stringBuilder.AppendLine(new string(' ', depth) + Error.ComponentFieldTypeIsEmpty());

            var fieldInfo = obj.GetType().GetField(fieldValue.name, bindingFlags);
            if (fieldInfo == null)
            {
                var warningMessage = $"[Error] Field '{fieldValue.name}' not found. Can't modify field '{fieldValue.name}'.";
                if (McpPluginUnity.IsLogActive(LogLevel.Warning))
                    Debug.LogWarning(warningMessage); //, go);
                return stringBuilder.AppendLine(new string(' ', depth) + warningMessage);
            }
            var targetType = TypeUtils.GetType(fieldValue.type);
            if (targetType == null)
            {
                if (McpPluginUnity.IsLogActive(LogLevel.Error))
                    Debug.LogError($"[Error] Type '{fieldValue.type}' not found. Can't modify field '{fieldValue.name}'."); //, go);
                return stringBuilder.AppendLine(new string(' ', depth) + Error.InvalidComponentFieldType(fieldValue, fieldInfo));
            }

            // The `targetType` is a UnityEngine.Object type, so it should be handled differently.
            if (typeof(UnityEngine.Object).IsAssignableFrom(targetType))
            {
                // Not support outside of UnityEditor for now.
#if UNITY_EDITOR
                var referenceInstanceID = JsonUtils.Deserialize<InstanceID>(fieldValue.valueJsonElement.Value).instanceID;
                if (referenceInstanceID == 0)
                    return stringBuilder.AppendLine(new string(' ', depth) + Error.InvalidInstanceID(targetType, fieldValue.name));

                // Find the object by instanceID
                var referenceObject = UnityEditor.EditorUtility.InstanceIDToObject(referenceInstanceID);

                if (typeof(Sprite).IsAssignableFrom(targetType) && referenceObject is Texture2D texture)
                {
                    // Try to find the first Sprite sub-asset in the Texture2D asset
                    var assetPath = UnityEditor.AssetDatabase.GetAssetPath(texture);
                    var sprite = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(assetPath)
                        .OfType<Sprite>()
                        .FirstOrDefault();
                    referenceObject = sprite;
                    fieldInfo.SetValue(obj, referenceObject);
                    return stringBuilder.AppendLine(new string(' ', depth) + $"[Success] Field '{fieldValue.name}' modified to '{referenceObject}'.");
                }
                else
                {
                    // Cast the object to the target type
                    var castedObject = TypeUtils.CastTo(referenceObject, targetType, out var error);
                    if (error != null)
                        return stringBuilder.AppendLine(new string(' ', depth) + error);

                    Modify(ref castedObject, fieldValue, stringBuilder, depth + 1, bindingFlags);

                    fieldInfo.SetValue(obj, castedObject);
                    return stringBuilder.AppendLine(new string(' ', depth) + $"[Success] Field '{fieldValue.name}' modified to '{castedObject}'.");
                }
#else
                var message = $"[Error] Field '{fieldValue.name}' modification failed: {Error.NotSupportedInRuntime(targetType)}";
                stringBuilder.AppendLine(new string(' ', depth) + message);
                if (McpPluginUnity.IsLogActive(LogLevel.Error))
                    Debug.LogError(message); //, go);
                return stringBuilder;
#endif
            }
            else
            {
                try
                {
                    var value = JsonUtils.Deserialize(fieldValue.valueJsonElement.Value.GetRawText(), targetType);
                    fieldInfo.SetValue(obj, value);
                    return stringBuilder.AppendLine(new string(' ', depth) + $"[Success] Field '{fieldValue.name}' modified to '{value}'.");
                }
                catch (Exception ex)
                {
                    var message = $"[Error] Field '{fieldValue.name}' modification failed: {ex.Message}";
                    if (McpPluginUnity.IsLogActive(LogLevel.Error))
                        Debug.LogError(message); //, go);
                    return stringBuilder.AppendLine(new string(' ', depth) + message);
                }
            }
        }

        public static StringBuilder ModifyProperty(ref object obj, SerializedMember propertyValue, StringBuilder stringBuilder = null, int depth = 0,
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
        {
            stringBuilder ??= new StringBuilder();

            if (string.IsNullOrEmpty(propertyValue.name))
                return stringBuilder.AppendLine(new string(' ', depth) + Error.ComponentPropertyNameIsEmpty());

            if (string.IsNullOrEmpty(propertyValue.type))
                return stringBuilder.AppendLine(new string(' ', depth) + Error.ComponentPropertyTypeIsEmpty());

            var propInfo = obj.GetType().GetProperty(propertyValue.name, bindingFlags);
            if (propInfo == null)
            {
                var warningMessage = $"[Error] Property '{propertyValue.name}' not found. Can't modify property '{propertyValue.name}'.";
                if (McpPluginUnity.IsLogActive(LogLevel.Warning))
                    Debug.LogWarning(warningMessage); //, go);
                return stringBuilder.AppendLine(new string(' ', depth) + warningMessage);
            }
            if (!propInfo.CanWrite)
            {
                var warningMessage = $"[Error] Property '{propertyValue.name}' is not writable. Can't modify property '{propertyValue.name}'.";
                if (McpPluginUnity.IsLogActive(LogLevel.Warning))
                    Debug.LogWarning(warningMessage); //, go);
                return stringBuilder.AppendLine(new string(' ', depth) + warningMessage);
            }

            var targetType = TypeUtils.GetType(propertyValue.type);
            if (targetType == null)
            {
                if (McpPluginUnity.IsLogActive(LogLevel.Error))
                    Debug.LogError($"[Error] Type '{propertyValue.type}' not found. Can't modify property '{propertyValue.name}'."); //, go);
                return stringBuilder.AppendLine(new string(' ', depth) + Error.InvalidComponentPropertyType(propertyValue, propInfo));
            }

            // The `targetType` is a UnityEngine.Object type, so it should be handled differently.
            if (typeof(UnityEngine.Object).IsAssignableFrom(targetType))
            {
                // Not support outside of UnityEditor for now.
#if UNITY_EDITOR
                var referenceInstanceID = JsonUtils.Deserialize<InstanceID>(propertyValue.valueJsonElement.Value).instanceID;
                if (referenceInstanceID == 0)
                    return stringBuilder.AppendLine(new string(' ', depth) + Error.InvalidInstanceID(targetType, propertyValue.name));

                // Find the object by instanceID
                var referenceObject = UnityEditor.EditorUtility.InstanceIDToObject(referenceInstanceID);

                if (typeof(Sprite).IsAssignableFrom(targetType) && referenceObject is Texture2D texture)
                {
                    // Try to find the first Sprite sub-asset in the Texture2D asset
                    var assetPath = UnityEditor.AssetDatabase.GetAssetPath(texture);
                    var sprite = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(assetPath)
                        .OfType<Sprite>()
                        .FirstOrDefault();
                    referenceObject = sprite;
                    propInfo.SetValue(obj, referenceObject);
                    return stringBuilder.AppendLine(new string(' ', depth) + $"[Success] Property '{propertyValue.name}' modified to '{referenceObject}'.");
                }
                else
                {
                    // Cast the object to the target type
                    var castedObject = TypeUtils.CastTo(referenceObject, targetType, out var error);
                    if (error != null)
                        return stringBuilder.AppendLine(new string(' ', depth) + error);

                    Modify(ref castedObject, propertyValue, stringBuilder, depth + 1, bindingFlags);

                    propInfo.SetValue(obj, castedObject);
                    return stringBuilder.AppendLine(new string(' ', depth) + $"[Success] Property '{propertyValue.name}' modified to '{castedObject}'.");
                }
#else
                var message = $"[Error] Property '{propertyValue.name}' modification failed: {Error.NotSupportedInRuntime(targetType)}";
                stringBuilder.AppendLine(new string(' ', depth) + message);
                if (McpPluginUnity.IsLogActive(LogLevel.Error))
                    Debug.LogError(message); //, go);
                return stringBuilder;
#endif
            }
            else
            {
                try
                {
                    var value = JsonUtils.Deserialize(propertyValue.valueJsonElement.Value.GetRawText(), targetType);
                    propInfo.SetValue(obj, value);
                    return stringBuilder.AppendLine(new string(' ', depth) + $"[Success] Property '{propertyValue.name}' modified to '{value}'.");
                }
                catch (Exception ex)
                {
                    var message = $"[Error] Property '{propertyValue.name}' modification failed: {ex.Message}";
                    if (McpPluginUnity.IsLogActive(LogLevel.Error))
                        Debug.LogError(message); //, go);
                    return stringBuilder.AppendLine(new string(' ', depth) + message);
                }
            }
        }
    }
}