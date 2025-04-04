#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace com.IvanMurzak.UnityMCP.Common.API
{
    public partial class Connector : IConnector
    {
        [MenuItem("Tools/MCP/Connect")]
        static void Editor_Connect()
        {
            Instance.Connect();
        }

        [MenuItem("Tools/MCP/Disconnect")]
        static void Editor_Disconnect()
        {
            if (!HasInstance)
            {
                Debug.Log($"{Consts.Log.Tag} {Consts.Log.Color.CategoryStart}[Connector]{Consts.Log.Color.CategoryEnd} Already disconnected.");
                return;
            }
            Instance.Disconnect();
        }
    }
}
#endif