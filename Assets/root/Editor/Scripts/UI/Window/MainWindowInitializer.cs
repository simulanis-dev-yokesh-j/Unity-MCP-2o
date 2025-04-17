using System.Collections;
using UnityEditor;

namespace com.IvanMurzak.Unity.MCP.Editor
{
    public static class MainWindowInitializer
    {
        [InitializeOnLoadMethod]
        public static IEnumerator Init()
        {
            yield return null; // let's Unity initialize itself and project resources first
            McpPluginUnity.Init(); // it triggers the loading of the config
            MainWindowEditor.ShowWindow();
        }
    }
}