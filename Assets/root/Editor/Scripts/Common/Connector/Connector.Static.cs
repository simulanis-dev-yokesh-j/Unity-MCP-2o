using UnityEngine;
using UnityEditor;

namespace com.IvanMurzak.UnityMCP.Common.API
{
    public partial class Connector : IConnector
    {
        static Connector instance;

        public static bool HasInstance => instance != null;
        public static Connector Instance => instance; // ??= new Connector();

        [MenuItem("Tools/MCP/Start Server")]
        static void Editor_Connect()
        {
            Instance.Connect();
        }

        [MenuItem("Tools/MCP/Connect Server")]
        static void Editor_Disconnect()
        {
            Debug.Log($"{Consts.Log.Tag} [Connector] Stopping");
            if (!HasInstance)
            {
                Debug.Log($"{Consts.Log.Tag} [Connector] Already stopped");
                return;
            }
            Instance.Disconnect();
        }
    }
}