using System.Collections;
using UnityEditor;

namespace com.IvanMurzak.Unity.MCP.Editor
{
    public static class ThemeInitializer
    {
        [InitializeOnLoadMethod]
        public static IEnumerator Init()
        {
            yield return null; // let's Unity initialize itself and project resources first
            var config = McpPluginUnity.Instance;
            MainWindowEditor.ShowWindow();
        }

        // [MenuItem("Edit/Unity-Theme/Reset Default Palettes")]
        // public static void ResetDefaultPalettes()
        // {
        //     var config = ConnectionConfig.Instance;

        //     config.RemoveAllThemes();
        //     config.RemoveAllColors();

        //     config.SetDefaultPalettes();

        //     MainWindowEditor.ShowWindow().Invalidate();
        // }
        // [MenuItem("Edit/Unity-Theme/Set Default Palettes")]
        // public static void SetDefaultPalettes()
        // {
        //     var config = ConnectionConfig.Instance;
        //     config.SetDefaultPalettes();

        //     MainWindowEditor.ShowWindow().Invalidate();
        // }
    }
}