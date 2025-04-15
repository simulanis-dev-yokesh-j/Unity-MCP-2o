using System;
using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Utils;
using Microsoft.AspNetCore.SignalR.Client;
using R3;
using UnityEngine;

namespace com.IvanMurzak.Unity.MCP
{
    public partial class McpPluginUnity
    {
        Data data = new Data();
        public event Action<Data> onChanged;

        private static McpPluginUnity instance;
        public static McpPluginUnity Instance
        {
            get
            {
                instance ??= GetOrCreateInstance();

                if (instance == null)
                    Debug.LogWarning("[McpPluginUnity] ConnectionConfig instance is null");

                return instance;
            }
        }
        public static bool IsLogLevelActive(LogLevel level)
            => Instance?.IsLogActive(level) ?? false;

        public bool IsLogActive(LogLevel level)
        {
            data ??= new Data();
            return data.logLevel.IsActive(level);
        }
        public LogLevel LogLevel
        {
            get => data?.logLevel ?? LogLevel.Trace;
            set
            {
                data ??= new Data();
                data.logLevel = value;
                NotifyChanged(data);
            }
        }
        public string Host
        {
            get => data?.host ?? Data.DefaultHost;
            set
            {
                data ??= new Data();
                data.host = value;
                NotifyChanged(data);
            }
        }
        public ReadOnlyReactiveProperty<HubConnectionState> ConnectionState => McpPlugin.Instance.ConnectionState;
        public ReadOnlyReactiveProperty<bool> IsConnected => McpPlugin.Instance.ConnectionState
            .Select(x => x == HubConnectionState.Connected)
            .ToReadOnlyReactiveProperty(false);

        public void OnValidate()
        {
            var changed = false;

            if (data.port < 0 || data.port > 65535)
            {
                data.port = Data.DefaultPort;
                changed = true;
            }

            if (string.IsNullOrEmpty(data.host))
            {
                data.host = Data.DefaultHost;
                changed = true;
            }

            if (changed)
                NotifyChanged(data);
        }

        protected virtual void NotifyChanged(Data data)
            => Safe.Run(onChanged, data, logLevel: data?.logLevel ?? LogLevel.Trace);
    }
}