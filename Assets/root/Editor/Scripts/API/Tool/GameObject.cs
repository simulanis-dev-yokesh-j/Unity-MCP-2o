#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using com.IvanMurzak.Unity.MCP.Common;
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
        }
    }
}