#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace com.IvanMurzak.UnityMCP.Common.API
{
    public partial class Connector : IConnector
    {
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
#endif