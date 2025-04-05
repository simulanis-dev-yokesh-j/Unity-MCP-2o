using UnityEngine;
using UnityEditor;
using Microsoft.Extensions.Logging;
using System.Net;
using com.IvanMurzak.UnityMCP.Common;
using com.IvanMurzak.UnityMCP.Common.API;

namespace com.IvanMurzak.UnityMCP.Editor
{
    [InitializeOnLoad]
    static class Startup
    {
        static Startup()
        {
            BuildAndStart();
        }

        [MenuItem("Tools/UnityMCP/Build and Start")]
        public static void BuildAndStart()
        {
            var message = "<b><color=yellow>STARTUP</color></b>";
            Debug.Log($"{Consts.Log.Tag} {message} <color=orange>╭━━━━╮┈┈┈┈╭━━━━╮┈┈┈┈╭━━━━╮</color>");

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
                    // loggingBuilder.AddSimpleConsole(options =>
                    // {
                    //     options.IncludeScopes = false;
                    //     options.SingleLine = true;
                    //     options.TimestampFormat = "hh:mm:ss ";
                    // });
                    loggingBuilder.AddProvider(new UnityLoggerProvider());
                    loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                })
                .WithCommandsFromAssembly(typeof(Startup).Assembly)
                .Build()
                .Connect();
        }
    }
}