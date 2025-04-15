using R3;
using UnityEditor;
using UnityEngine;

namespace com.IvanMurzak.Unity.MCP.Editor
{
    public partial class MainWindowEditor : EditorWindow
    {
        readonly CompositeDisposable _disposables = new();

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

        private void OnChanged(McpPluginUnity.Data data) => Repaint();

        private void OnEnable()
        {
            McpPluginUnity.Instance.onChanged += OnChanged;
        }
        private void OnDisable()
        {
            McpPluginUnity.Instance.onChanged -= OnChanged;
            _disposables.Clear();
        }
    }
}