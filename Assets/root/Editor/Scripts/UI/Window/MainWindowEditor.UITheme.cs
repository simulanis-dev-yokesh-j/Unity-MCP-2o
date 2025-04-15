using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;
using System;

namespace com.IvanMurzak.Unity.MCP.Editor
{
    public partial class MainWindowEditor : EditorWindow
    {
        // class UITheme : IDisposable
        // {
        //     public VisualElement root;
        //     public TextField textFieldName;
        //     public Button btnDelete;
        //     public Toggle toggleFoldout;
        //     public ListView listColors;
        //     public VisualElement contPreview;
        //     public VisualElement contContent;
        //     public ThemeData theme;
        //     public Dictionary<string, UIThemeColor> colors;

        //     public void RebuildColors(McpPluginUnity config)
        //     {
        //         RebuildColorPreviews(config);
        //         listColors.Rebuild();
        //     }
        //     public void RebuildColorPreviews(McpPluginUnity config)
        //     {
        //         contPreview.Clear();
        //         var colorFillTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(AssetDatabase.GUIDToAssetPath(colorFillTemplateGuid));
        //         foreach (var colorRef in config.GetColors())
        //         {
        //             var themeColor = theme.GetColorByRef(colorRef);
        //             var colorFill = colorFillTemplate.Instantiate();
        //             colorFill.Query<VisualElement>("colorFill").Last().style.unityBackgroundImageTintColor = new StyleColor(themeColor.Color);
        //             contPreview.Add(colorFill);
        //         }
        //     }

        //     public void Dispose()
        //     {
        //         colors?.Clear();
        //         colors = null;
        //     }
        // }
    }
}