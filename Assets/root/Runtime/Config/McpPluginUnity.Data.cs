using System;
using UnityEngine;

namespace com.IvanMurzak.Unity.MCP
{
    public partial class McpPluginUnity
    {
        [Serializable]
        public class Data
        {
            public const int DefaultPort = 60606;
            public const string DefaultHost = "http://localhost";

            [SerializeField] public string host = DefaultHost;
            [SerializeField] public int port = DefaultPort;
            [SerializeField] public LogLevel logLevel = LogLevel.Warning;

            public Data SetDefault()
            {
                host = DefaultHost;
                port = DefaultPort;
                logLevel = LogLevel.Warning;
                return this;
            }
        }
    }
}