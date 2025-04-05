using UnityEditor;
using Microsoft.Extensions.Logging;
using System.Net;
using com.IvanMurzak.UnityMCP.Common;
using com.IvanMurzak.UnityMCP.Common.API;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System.IO;
using UnityEngine;

namespace com.IvanMurzak.UnityMCP.Editor
{
    [InitializeOnLoad]
    static class Startup
    {
        static string ServerRootPath => Path.GetFullPath(Path.Combine(Application.dataPath, "../Packages/com.IvanMurzak.UnityMCP.Server"));
        static string ServerExecutablePath => Path.Combine(ServerRootPath, "bin/Release/net9.0/com.IvanMurzak.UnityMCP.Server.exe");
        static bool IsServerCompiled => File.Exists(ServerExecutablePath);

        static Startup()
        {
            BuildAndStart();
            CompileServerIfNeeded();
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
                    loggingBuilder.AddProvider(new UnityLoggerProvider());
                    loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                })
                .WithCommandsFromAssembly(typeof(Startup).Assembly)
                .Build()
                .Connect();
        }

        public static void CompileServerIfNeeded()
        {
            if (IsServerCompiled)
                return;
            CompileServer();
        }

        [MenuItem("Tools/UnityMCP/Compile server")]
        public static void CompileServer()
        {
            var message = "<b><color=yellow>Server Compilation</color></b>";
            Debug.Log($"{Consts.Log.Tag} {message} <color=orange>╰━━━━╯┈┈┈┈╰━━━━╯┈┈┈┈╰━━━━╯</color>");

            var processStartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = "build -c Release",
                WorkingDirectory = ServerRootPath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = new Process { StartInfo = processStartInfo })
            {
                process.Start();

                // Read the output and error streams
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                // Log the results
                Debug.Log($"{Consts.Log.Tag} Build Output:\n{output}");
                if (!string.IsNullOrEmpty(error))
                {
                    Debug.LogError($"{Consts.Log.Tag} Build Errors:\n{error}");
                }
            }
        }
    }
}