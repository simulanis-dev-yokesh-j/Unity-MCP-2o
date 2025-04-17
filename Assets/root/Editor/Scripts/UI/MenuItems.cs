#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using com.IvanMurzak.Unity.MCP.Common;
using System.Threading.Tasks;

namespace com.IvanMurzak.Unity.MCP.Editor
{
    public static class MenuItems
    {
        [MenuItem("Tools/Unity-MCP/Build and Start", priority = 1000)]
        public static void BuildAndStart() => Startup.BuildAndStart();

        [MenuItem("Tools/Unity-MCP/Connect", priority = 1001)]
        public static void Editor_Connect() => McpPlugin.Instance.Connect();

        [MenuItem("Tools/Unity-MCP/Disconnect", priority = 1001)]
        public static void Editor_Disconnect()
        {
            if (!McpPlugin.HasInstance)
            {
                Debug.Log($"{Consts.Log.Tag} {Consts.Log.Color.CategoryStart}Connector{Consts.Log.Color.CategoryEnd} Already disconnected.");
                return;
            }
            McpPlugin.Instance.Disconnect();
        }

        [MenuItem("Tools/Unity-MCP/Server/Build", priority = 1010)]
        public static Task CompileServer() => Startup.BuildServer();

        [MenuItem("Tools/Unity-MCP/Server/Open Logs", priority = 1011)]
        public static void OpenLogs()
        {
            if (System.IO.File.Exists(Startup.ServerLogsPath))
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = Startup.ServerLogsPath,
                    UseShellExecute = true // Ensures the file opens with the default application
                });
            }
            else
            {
                Debug.LogError($"{Consts.Log.Tag} Log file not found at: {Startup.ServerLogsPath}");
            }
        }
    }
}
#endif