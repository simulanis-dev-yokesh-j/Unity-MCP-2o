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
            var config = McpPluginUnity.Instance;
            MainWindowEditor.ShowWindow();
        }
    }
}