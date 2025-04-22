#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using com.IvanMurzak.Unity.MCP.Common;
using UnityEditor;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    [McpPluginToolType]
    public partial class Tool_Assets_Prefab
    {
        public static class Error
        {
            static string PrefabsPrinted => string.Join("\n", AssetDatabase.FindAssets("t:Prefab"));

            public static string PrefabPathIsEmpty()
                => "[Error] Prefab path is empty. Available prefabs:\n" + PrefabsPrinted;

            public static string NotFoundPrefabAtPath(string path)
                => $"[Error] Prefab '{path}' not found. Available prefabs:\n" + PrefabsPrinted;

            public static string PrefabPathIsInvalid(string path)
                => $"[Error] Prefab path '{path}' is invalid.";
        }
    }
}