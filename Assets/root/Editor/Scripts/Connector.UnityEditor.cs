#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Common.API;

namespace com.IvanMurzak.Unity.MCP.Editor
{
    public static class ConnectorEditor
    {
        [MenuItem("Tools/Unity-MCP/Connect", priority = 1001)]
        static void Editor_Connect()
        {
            Connector.Instance.Connect();
        }

        [MenuItem("Tools/Unity-MCP/Disconnect", priority = 1001)]
        static void Editor_Disconnect()
        {
            if (!Connector.HasInstance)
            {
                Debug.Log($"{Consts.Log.Tag} {Consts.Log.Color.CategoryStart}Connector{Consts.Log.Color.CategoryEnd} Already disconnected.");
                return;
            }
            Connector.Instance.Disconnect();
        }
    }
}
#endif