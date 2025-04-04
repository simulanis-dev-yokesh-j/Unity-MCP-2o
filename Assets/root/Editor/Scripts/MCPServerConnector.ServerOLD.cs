// using UnityEngine;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Hosting;
// using Microsoft.Extensions.Logging;
// using ModelContextProtocol.Server;
// using System.ComponentModel;
// using UnityEditor;
// using System.Threading.Tasks;
// using System.Threading;
// using Microsoft.Extensions.Options;
// using System.Net;
// using ModelContextProtocol.Client;
// using ModelContextProtocol.Protocol.Transport;
// using System.Collections.Generic;

// namespace com.IvanMurzak.UnityMCP.Editor
// {
//     [InitializeOnLoad]
//     public partial class MCPServerConnector
//     {
//         public const string Version = "0.1.0";

//         static void StartOLD()
//         {
//             Debug.Log("[MCP Server] <color=red>---------------------------------------------</color>");
//             if (runningServer?.Status == TaskStatus.WaitingToRun || runningServer?.Status == TaskStatus.Running)
//             {
//                 Debug.Log("[MCP Server] already running");
//                 return;
//             }
//             new Thread(() =>
//             {
//                 var builder = Host.CreateApplicationBuilder();
//                 builder.Logging.ClearProviders(); // 👈 Clears the default providers including EventLog
//                 builder.Logging.AddSimpleConsole(options =>
//                 {
//                     options.IncludeScopes = false;
//                     options.SingleLine = true;
//                     options.TimestampFormat = "hh:mm:ss ";
//                 });
//                 builder.Logging.AddProvider(new UnityLoggerProvider());
//                 builder.Logging.SetMinimumLevel(LogLevel.Trace);
//                 // builder.Logging.AddConsole(consoleLogOptions =>
//                 // {
//                 //     // Configure all logs to go to stderr
//                 //     consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Error;
//                 // });
//                 builder.Services
//                     .AddMcpServer(configureOptions =>
//                     {
//                         configureOptions.ServerInfo.Name = "Unity MCP Server";
//                         configureOptions.ServerInfo.Version = Version;
//                         configureOptions.ServerInstructions = "This is a Unity Model Context Protocol Server. It is used to communicate with the Unity Model Context Protocol Client.";
//                         // configureOptions.Capabilities.Tools = new ToolsCapability
//                         // {
//                         // }
//                     })
//                     .WithTcpServerTransport(options =>
//                     {
//                         options.Port = 55555;
//                     })
//                     .WithToolsFromAssembly();


//                 var app = builder.Build();
//                 Debug.Log("[MCP Server] Starting");
//                 runningServer = app.RunAsync();
//                 runningServer.ContinueWith(task =>
//                 {
//                     if (task.IsFaulted)
//                     {
//                         Debug.LogError("[MCP Server] failed: " + task.Exception?.Message);
//                         if (task.Exception != null)
//                             Debug.LogException(task.Exception);
//                     }
//                     else
//                     {
//                         Debug.Log("[MCP Server] Stopped");
//                     }
//                 });
//                 Debug.Log("[MCP Server] Started");
//             }).Start();
//         }
//     }

//     [McpServerToolType]
//     public static class EchoTool
//     {
//         [McpServerTool, Description("Echoes the message back to the client.")]
//         public static string Echo(string message) => $"hello {message}";
//     }
// }