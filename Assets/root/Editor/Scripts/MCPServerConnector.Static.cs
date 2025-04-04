using UnityEngine;
using UnityEditor;

namespace com.IvanMurzak.UnityMCP.Editor
{
    public partial class MCPServerConnector
    {
        static MCPServerConnector instance;

        public static MCPServerConnector Instance => instance ??= new MCPServerConnector();

        static MCPServerConnector()
        {
            Connect();
            EditorApplication.quitting += Disconnect;
        }
        [MenuItem("Tools/MCP/Start Server")]
        static void Connect()
        {
            if (Instance.IsConnected)
            {
                Debug.Log("[MCP Server] already running");
                return;
            }
            Instance.Start();
        }
        [MenuItem("Tools/MCP/Connect Server")]
        static void Disconnect()
        {
            Debug.Log("[MCP Server] Stopping");
        }
    }
}