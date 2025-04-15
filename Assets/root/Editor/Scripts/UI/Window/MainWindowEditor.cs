using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace com.IvanMurzak.Unity.MCP.Editor
{
    public partial class MainWindowEditor : EditorWindow
    {
        [SerializeField] VisualTreeAsset templateControlPanel;

        [MenuItem("Window/AI Connector (Unity-MCP)")]
        public static MainWindowEditor ShowWindow()
        {
            var window = GetWindow<MainWindowEditor>();
            window.titleContent = new GUIContent("AI Connector");
            window.Focus();

            return window;
        }
        public static void ShowWindowVoid() => ShowWindow();

        public void Invalidate() => CreateGUI();
        void OnValidate() => McpPluginUnity.Instance.OnValidate();

        private void SaveChanges(string message)
        {
            if (McpPluginUnity.IsLogLevelActive(LogLevel.Log))
                Debug.Log(message);
            saveChangesMessage = message;

            Undo.RecordObject(McpPluginUnity.Instance.AssetFile, message); // Undo record started
            base.SaveChanges();
            McpPluginUnity.Instance.Save();
            McpPluginUnity.Instance.InvalidateAssetFile();
            EditorUtility.SetDirty(McpPluginUnity.Instance.AssetFile); // Undo record completed
        }

        private void UpdateDropdownCurrentTheme(McpPluginUnity config)
        {
            // dropdownCurrentTheme.choices = config.ThemeNames.ToList();
            // dropdownCurrentTheme.value = config.CurrentThemeName;
        }
        private void OnChanged(McpPluginUnity.Data data) => Repaint();

        private void OnEnable()
        {
            McpPluginUnity.Instance.onChanged += OnChanged;
        }
        private void OnDisable()
        {
            McpPluginUnity.Instance.onChanged -= OnChanged;
        }
        public void CreateGUI()
        {
            rootVisualElement.Clear();
            if (templateControlPanel == null)
            {
                Debug.LogError("'templateControlPanel' is not assigned. Please assign it in the inspector.");
                return;
            }

            VisualElement ui = templateControlPanel.Instantiate();

            var config = McpPluginUnity.Instance;
            var root = templateControlPanel.Instantiate();
            rootVisualElement.Add(root);

            // uiThemes.Clear();
            // uiThemeColors.Clear();

            // Settings
            // -----------------------------------------------------------------

            var dropdownLogLevel = root.Query<EnumField>("dropdownLogLevel").First();

            UpdateDropdownCurrentTheme(config);

            dropdownLogLevel.RegisterValueChangedCallback(evt =>
            {
                SetLogLevel(evt.newValue as LogLevel? ?? LogLevel.Warning);
                SaveChanges($"[AI Connector] LogLevel Changed: {evt.newValue}");
            });

            // enumDebugLevel.value = config.debugLevel;
            // enumDebugLevel.RegisterValueChangedCallback(evt =>
            // {
            //     config.debugLevel = (DebugLevel)evt.newValue;
            //     SaveChanges($"[Theme] Debug status changed: {evt.newValue}");
            // });

            // Themes
            // -----------------------------------------------------------------

            var inputFieldNewThemeName = root
                .Query<VisualElement>("contHeaderThemes").First()
                .Query<TextField>("textFieldNewName").First();

            var btnCreateNewTheme = root
                .Query<VisualElement>("contHeaderThemes").First()
                .Query<Button>("btnCreateNew").First();

            var rootThemes = root
                .Query<VisualElement>("rootThemes").First();

            // btnCreateNewTheme.RegisterCallback<ClickEvent>(evt =>
            // {
            //     var themeName = inputFieldNewThemeName.value;
            //     inputFieldNewThemeName.value = "New Theme";
            //     var theme = config.AddTheme(themeName);

            //     UpdateDropdownCurrentTheme(config);

            //     UIAddTheme(config, rootThemes, theme);
            //     SaveChanges($"[Theme] Theme added: {themeName}");
            // });

            // foreach (var theme in config.Themes)
            //     UIAddTheme(config, rootThemes, theme);
        }
        void SetLogLevel(LogLevel logLevel)
        {
            data.logLevel = logLevel;
            McpPluginUnity.Instance. = logLevel;
        }
    }
}