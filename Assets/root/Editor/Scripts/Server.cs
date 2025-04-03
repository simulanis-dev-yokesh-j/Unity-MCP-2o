using UnityEngine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using System.ComponentModel;
using UnityEditor;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Options;

namespace com.IvanMurzak.UnityMCP.Editor
{
    [InitializeOnLoad]
    public static class Server
    {
        public const string Version = "0.1.0";

        static Task runningServer;

        static Server()
        {
            Start();
            EditorApplication.quitting += Stop;
        }
        [MenuItem("Tools/MCP/Start Server")]
        static void Start()
        {
            Debug.Log("[MCP Server] <color=red>---------------------------------------------</color>");
            if (runningServer?.Status == TaskStatus.WaitingToRun || runningServer?.Status == TaskStatus.Running)
            {
                Debug.Log("[MCP Server] already running");
                return;
            }
            new Thread(() =>
            {
                var builder = Host.CreateApplicationBuilder();
                builder.Logging.ClearProviders(); // 👈 Clears the default providers including EventLog
                builder.Logging.AddSimpleConsole(options =>
                {
                    options.IncludeScopes = false;
                    options.SingleLine = true;
                    options.TimestampFormat = "hh:mm:ss ";
                });
                builder.Logging.AddProvider(new UnityLoggerProvider());
                builder.Logging.SetMinimumLevel(LogLevel.Trace);
                // builder.Logging.AddConsole(consoleLogOptions =>
                // {
                //     // Configure all logs to go to stderr
                //     consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Error;
                // });
                builder.Services
                    .AddMcpServer(configureOptions =>
                    {
                        configureOptions.ServerInfo.Name = "Unity MCP Server";
                        configureOptions.ServerInfo.Version = Version;
                        configureOptions.ServerInstructions = "This is a Unity Model Context Protocol Server. It is used to communicate with the Unity Model Context Protocol Client.";
                        // configureOptions.Capabilities.Tools = new ToolsCapability
                        // {
                        // }
                    })
                    .WithTcpServerTransport(options =>
                    {
                        options.Port = 5050; // Any free port
                        options.Host = "127.0.0.1";
                    })
                    //.WithStdioServerTransport()
                    .WithToolsFromAssembly();

                var app = builder.Build();
                Debug.Log("[MCP Server] Starting");
                runningServer = app.RunAsync();
                runningServer.ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.LogError("[MCP Server] failed: " + task.Exception?.Message);
                    }
                    else
                    {
                        Debug.Log("[MCP Server] Stopped");
                    }
                });
                Debug.Log("[MCP Server] Started");
            }).Start();
        }
        [MenuItem("Tools/MCP/Stop Server")]
        static void Stop()
        {
            Debug.Log("[MCP Server] Stopping");
        }
    }

    [McpServerToolType]
    public static class EchoTool
    {
        [McpServerTool, Description("Echoes the message back to the client.")]
        public static string Echo(string message) => $"hello {message}";
    }
}