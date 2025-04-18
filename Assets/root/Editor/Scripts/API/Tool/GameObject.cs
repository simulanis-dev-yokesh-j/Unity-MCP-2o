#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Common.Data.Utils;
using com.IvanMurzak.Unity.MCP.Editor.Utils;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    [McpPluginToolType]
    public partial class Tool_GameObject
    {
        public static class Error
        {
            static string RootGOsPrinted => GameObjectUtils.FindRootGameObjects().Print();

            public static string GameObjectPathIsEmpty()
                => $"[Error] GameObject path is empty. Root GameObjects in the active scene:\n{RootGOsPrinted}";
            public static string NotFoundGameObjectAtPath(string path)
                => $"[Error] GameObject '{path}' not found. Root GameObjects in the active scene:\n{RootGOsPrinted}";

            public static string GameObjectInstanceIdIsEmpty()
                => $"[Error] GameObject InstanceId is empty. Root GameObjects in the active scene:\n{RootGOsPrinted}";
            public static string GameObjectNameIsEmpty()
                => $"[Error] GameObject name is empty. Root GameObjects in the active scene:\n{RootGOsPrinted}";
            public static string NotFoundGameObjectWithName(string name)
                => $"[Error] GameObject with name '{name}' not found. Root GameObjects in the active scene:\n{RootGOsPrinted}";
            public static string NotFoundGameObjectWithInstanceId(int instanceId)
                => $"[Error] GameObject with InstanceId '{instanceId}' not found. Root GameObjects in the active scene:\n{RootGOsPrinted}";

            public static string TypeMismatch(string typeName, string expectedTypeName)
                => $"[Error] Type mismatch. Expected '{expectedTypeName}', but got '{typeName}'.";
            public static string InvalidComponentPropertyType(SerializedMember serializedProperty, PropertyInfo propertyInfo)
                => $"[Error] Invalid component property type '{serializedProperty.type}' for '{propertyInfo.Name}'. Expected '{propertyInfo.PropertyType.FullName}'.";
            public static string InvalidComponentFieldType(SerializedMember serializedProperty, FieldInfo propertyInfo)
                => $"[Error] Invalid component property type '{serializedProperty.type}' for '{propertyInfo.Name}'. Expected '{propertyInfo.FieldType.FullName}'.";
            public static string InvalidComponentType(string typeName)
                => $"[Error] Invalid component type '{typeName}'. It should be a valid Component Type.";
            public static string NotFoundComponent(int componentInstanceId, IEnumerable<UnityEngine.Component> allComponents)
            {
                var availableComponentsPreview = allComponents
                    .Select(c => MCP.Utils.Serializer.Component.BuildDataLight(c))
                    .ToList();
                var previewJson = JsonUtils.Serialize(availableComponentsPreview);

                return $"[Error] No component with instanceId '{componentInstanceId}' found in GameObject.\nAvailable components preview:\n{previewJson}";
            }
            public static string NotFoundComponents(int[] componentInstanceIds, IEnumerable<UnityEngine.Component> allComponents)
            {
                var componentInstanceIdsString = string.Join(", ", componentInstanceIds);
                var availableComponentsPreview = allComponents
                    .Select(c => MCP.Utils.Serializer.Component.BuildDataLight(c))
                    .ToList();
                var previewJson = JsonUtils.Serialize(availableComponentsPreview);

                return $"[Error] No components with instanceIds [{componentInstanceIdsString}] found in GameObject.\nAvailable components preview:\n{previewJson}";
            }
            public static string ComponentPropertyNameIsEmpty()
                => $"[Error] Component property name is empty. It should be a valid property name.";
            public static string ComponentPropertyTypeIsEmpty()
                => $"[Error] Component property type is empty. It should be a valid property type.";
        }
    }
}