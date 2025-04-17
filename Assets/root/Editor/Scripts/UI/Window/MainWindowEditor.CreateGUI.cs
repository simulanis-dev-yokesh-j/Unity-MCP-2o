using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using com.IvanMurzak.Unity.MCP.Common;
using Microsoft.AspNetCore.SignalR.Client;
using R3;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace com.IvanMurzak.Unity.MCP.Editor
{
    public partial class MainWindowEditor : EditorWindow
    {
        const string USS_IndicatorClass_Connected = "status-indicator-circle-online";
        const string USS_IndicatorClass_Connecting = "status-indicator-circle-connecting";
        const string USS_IndicatorClass_Disconnected = "status-indicator-circle-disconnected";

        const string ServerButtonText_Connect = "Connect";
        const string ServerButtonText_Disconnect = "Disconnect";
        const string ServerButtonText_Stop = "Stop";

        [SerializeField] VisualTreeAsset templateControlPanel;

        public void CreateGUI()
        {
            _disposables.Clear();
            rootVisualElement.Clear();
            if (templateControlPanel == null)
            {
                Debug.LogError("'templateControlPanel' is not assigned. Please assign it in the inspector.");
                return;
            }

            var root = templateControlPanel.Instantiate();
            rootVisualElement.Add(root);

            // Settings
            // -----------------------------------------------------------------

            var dropdownLogLevel = root.Query<EnumField>("dropdownLogLevel").First();
            dropdownLogLevel.value = McpPluginUnity.Instance.LogLevel;
            dropdownLogLevel.RegisterValueChangedCallback(evt =>
            {
                McpPluginUnity.Instance.LogLevel = evt.newValue as LogLevel? ?? LogLevel.Warning;
                SaveChanges($"[AI Connector] LogLevel Changed: {evt.newValue}");
                Startup.BuildAndStart();
            });

            // Connection status
            // -----------------------------------------------------------------

            var inputFieldHost = root.Query<TextField>("InputServerURL").First();
            inputFieldHost.value = McpPluginUnity.Instance.Host;
            inputFieldHost.RegisterValueChangedCallback(evt =>
            {
                McpPluginUnity.Instance.Host = evt.newValue;
                SaveChanges($"[AI Connector] Host Changed: {evt.newValue}");
            });

            var btnConnectOrDisconnect = root.Query<Button>("btnConnectOrDisconnect").First();
            var connectionStatusCircle = root
                .Query<VisualElement>("ServerConnectionInfo").First()
                .Query<VisualElement>("connectionStatusCircle").First();
            var connectionStatusText = root
                .Query<VisualElement>("ServerConnectionInfo").First()
                .Query<Label>("connectionStatusText").First();

            Observable.CombineLatest(
                    McpPluginUnity.Instance.ConnectionState,
                    McpPlugin.Instance.KeepConnected,
                    (connectionState, keepConnected) => (connectionState, keepConnected)
                )
                .Subscribe(tuple =>
                {
                    var (connectionState, keepConnected) = tuple;

                    inputFieldHost.isReadOnly = keepConnected || connectionState switch
                    {
                        HubConnectionState.Connected => true,
                        HubConnectionState.Disconnected => false,
                        HubConnectionState.Reconnecting => true,
                        _ => false
                    };

                    connectionStatusCircle.RemoveFromClassList(USS_IndicatorClass_Connected);
                    connectionStatusCircle.RemoveFromClassList(USS_IndicatorClass_Connecting);
                    connectionStatusCircle.RemoveFromClassList(USS_IndicatorClass_Disconnected);

                    connectionStatusCircle.AddToClassList(connectionState switch
                    {
                        HubConnectionState.Connected => USS_IndicatorClass_Connected,
                        HubConnectionState.Disconnected => keepConnected
                            ? USS_IndicatorClass_Connecting
                            : USS_IndicatorClass_Disconnected,
                        HubConnectionState.Reconnecting => USS_IndicatorClass_Connecting,
                        _ => throw new ArgumentOutOfRangeException(nameof(connectionState), connectionState, null)
                    });

                    connectionStatusText.text = connectionState switch
                    {
                        HubConnectionState.Connected => "Connected",
                        HubConnectionState.Disconnected => keepConnected
                            ? "Connecting..."
                            : "Disconnected",
                        HubConnectionState.Reconnecting => "Reconnecting...",
                        _ => McpPluginUnity.Instance.IsConnected.CurrentValue.ToString() ?? "Unknown"
                    };

                    btnConnectOrDisconnect.text = connectionState switch
                    {
                        HubConnectionState.Connected => ServerButtonText_Disconnect,
                        HubConnectionState.Disconnected => keepConnected
                            ? ServerButtonText_Stop
                            : ServerButtonText_Connect,
                        HubConnectionState.Reconnecting => ServerButtonText_Stop,
                        _ => McpPluginUnity.Instance.IsConnected.CurrentValue.ToString() ?? "Unknown"
                    };
                })
                .AddTo(_disposables);

            btnConnectOrDisconnect.RegisterCallback<ClickEvent>(evt =>
            {
                if (btnConnectOrDisconnect.text == ServerButtonText_Connect)
                {
                    btnConnectOrDisconnect.text = ServerButtonText_Stop;
                    McpPluginUnity.Instance.KeepConnected = true;
                    McpPluginUnity.Instance.Save();
                    if (McpPlugin.HasInstance)
                    {
                        McpPlugin.Instance.Connect();
                    }
                    else
                    {
                        Startup.BuildAndStart();
                    }
                }
                else if (btnConnectOrDisconnect.text == ServerButtonText_Disconnect)
                {
                    btnConnectOrDisconnect.text = ServerButtonText_Connect;
                    McpPluginUnity.Instance.KeepConnected = false;
                    McpPluginUnity.Instance.Save();
                    if (McpPlugin.HasInstance)
                        McpPlugin.Instance.Disconnect();
                }
                else if (btnConnectOrDisconnect.text == ServerButtonText_Stop)
                {
                    btnConnectOrDisconnect.text = ServerButtonText_Connect;
                    McpPluginUnity.Instance.KeepConnected = false;
                    McpPluginUnity.Instance.Save();
                    if (McpPlugin.HasInstance)
                        McpPlugin.Instance.Disconnect();
                }
            });

            // Configure MCP Client
            // -----------------------------------------------------------------

#if UNITY_EDITOR_WIN
            ConfigureClientsWindows(root);
#elif UNITY_EDITOR_OSX || UNITY_EDITOR_LINUX
            ConfigureClientsMacAndLinux(root);
#endif
            // Provide raw json configuration
            // -----------------------------------------------------------------

            var rawJsonField = root.Query<TextField>("rawJsonConfiguration").First();
            rawJsonField.value = Startup.RawJsonConfiguration(McpPluginUnity.Instance.Port);
        }
    }
}