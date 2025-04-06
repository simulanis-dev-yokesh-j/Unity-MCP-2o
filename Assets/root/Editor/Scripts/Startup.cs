using UnityEditor;
using Microsoft.Extensions.Logging;
using System.Net;
using com.IvanMurzak.Unity.MCP.Common;
using Debug = UnityEngine.Debug;

namespace com.IvanMurzak.Unity.MCP.Editor
{
    [InitializeOnLoad]
    static partial class Startup
    {
        const string PackageName = "com.ivanmurzak.unity.mcp";

        static Startup()
        {
            BuildAndStart();
            CompileServerIfNeeded();
        }

        public static void BuildAndStart()
        {
            var message = "<b><color=yellow>STARTUP</color></b>";
            Debug.Log($"{Consts.Log.Tag} {message} <color=orange>ಠ‿ಠ</color>");

            new ConnectorBuilder()
                .WithConfig(config =>
                {
                    config.IPAddress = IPAddress.Loopback;
                    config.PortServer = 60600;
                    config.PortUnity = 60606;
                    config.ConnectionType = Connector.ConnectionRole.Unity;
                })
                .AddLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders(); // 👈 Clears the default providers
                    loggingBuilder.AddProvider(new UnityLoggerProvider());
                    loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                })
                .WithToolsFromAssembly(typeof(Startup).Assembly)
                .WithPromptsFromAssembly(typeof(Startup).Assembly)
                .Build()
                .Connect();
        }
    }
}