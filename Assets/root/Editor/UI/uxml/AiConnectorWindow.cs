using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class AiConnectorWindow : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Window/UI Toolkit/AiConnectorWindow")]
    public static void ShowExample()
    {
        AiConnectorWindow wnd = GetWindow<AiConnectorWindow>();
        wnd.titleContent = new GUIContent("AiConnectorWindow");
    }

    public void CreateGUI()
    {
        rootVisualElement.Clear();
        if (m_VisualTreeAsset != null)
        {
            VisualElement ui = m_VisualTreeAsset.Instantiate();
            rootVisualElement.Add(ui);

            // Optionally set logo image here if you have one
            var logo = ui.Q<Image>("logo");
            // logo.image = ...; // assign your Texture2D here

            // Optionally set connection status dynamically
            var status = ui.Q<VisualElement>("connectionStatus");
            // status.style.backgroundColor = Color.green; // or update color/class as needed

            // Optionally set MCP server address dynamically
            var mcpServer = ui.Q<Label>("mcpServer");
            // mcpServer.text = $"MCP Server: \"{yourServerAddress}\"";

            // Optionally set the readonly text dynamically
            var readonlyText = ui.Q<TextField>("readonlyText");
            // readonlyText.value = "Your multi-line, read-only, selectable text goes here.";
        }
    }
}
