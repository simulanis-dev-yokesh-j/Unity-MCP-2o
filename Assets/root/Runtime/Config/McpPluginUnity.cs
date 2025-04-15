using System;
using com.IvanMurzak.Unity.MCP.Utils;
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
            if (data == null)
                return false;

            return data.logLevel.IsActive(level);
        }

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