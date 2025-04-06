#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace com.IvanMurzak.UnityMCP.Common.API
{
    public partial class Connector : IConnector
    {
        [MenuItem("Tools/Unity-MCP/Connect", priority = 1001)]
        static void Editor_Connect()
        {
            Instance.Connect();
        }

        [MenuItem("Tools/Unity-MCP/Disconnect", priority = 1001)]
        static void Editor_Disconnect()
        {
            if (!HasInstance)
            {
                Debug.Log($"{Consts.Log.Tag} {Consts.Log.Color.CategoryStart}Connector{Consts.Log.Color.CategoryEnd} Already disconnected.");
                return;
            }
            Instance.Disconnect();
        }
    }
}
#endif