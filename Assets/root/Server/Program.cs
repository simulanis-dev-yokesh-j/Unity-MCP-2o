#if !IGNORE
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using com.IvanMurzak.Unity.MCP.Common;
using NLog.Extensions.Logging;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using NLog;
using System;

namespace com.IvanMurzak.Unity.MCP.Server
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Configure NLog
            var logger = LogManager.Setup().LoadConfigurationFromFile("NLog.config").GetCurrentClassLogger();
            try
            {
                var builder = Host.CreateApplicationBuilder(args);
                builder.Logging.AddConsole(consoleLogOptions =>
                {
                    // Configure all logs to go to stderr
                    consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
                });
                // Replace default logging with NLog
                // builder.Logging.ClearProviders();
                builder.Logging.AddNLog();

                builder.Services
                    .AddMcpServer()
                    .WithStdioServerTransport()
                    .WithPromptsFromAssembly()
                    .WithToolsFromAssembly()
                    .WithListResourceTemplatesHandler(ResourceRouter.ListResourceTemplates)
                    .WithListResourcesHandler(ResourceRouter.ListResources)
                    .WithReadResourceHandler(ResourceRouter.ReadResource);

                builder.Services
                    .AddConnector()
                    .AddLogging(logging =>
                    {
                        logging.AddConsole(consoleLogOptions =>
                        {
                            // Ensure logs are sent to stdout
                            consoleLogOptions.FormatterName = ConsoleFormatterNames.Systemd;
                        });
                        logging.AddNLog();
                        logging.SetMinimumLevel(LogLevel.Information);
                    })
                    .WithConfig(config =>
                    {
                        config.ConnectionType = Connector.ConnectionRole.Server;
                    })
                    .Build()
                    .Connect();

                // var server = builder.Services.BuildServiceProvider().GetRequiredService<IMcpServer>();
                // if (server.ServerOptions.Capabilities?.Tools?.ToolCollection != null)
                // {
                //     server.ServerOptions.Capabilities.Tools.ToolCollection
                //         .Add(McpServerTool.Create(() =>
                //         {
                //             if (Connector.HasInstance)
                //             {
                //                 Connector.Instance?.Send("This is custom command from server!");
                //                 return Task.FromResult("This is custom response from server!");
                //             }
                //             return Task.FromResult("Connector is null");
                //         },
                //         new McpServerToolCreateOptions()
                //         {
                //             Name = "CustomCommand",
                //             Description = "This is custom command from server!"
                //         }));
                //     // server.ServerOptions.Capabilities.Tools.ListChanged = true;
                // }
                // //server.ClientCapabilities.Roots.ListChanged = true; // .Sampling
                // // server.SendMessageAsync("Hello from server!"); // .Wait();

                await builder.Build().RunAsync();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Application stopped due to an exception.");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }
    }
}
#endif