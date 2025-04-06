#if !IGNORE
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using com.IvanMurzak.Unity.MCP.Common;

namespace com.IvanMurzak.Unity.MCP.Server
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Logging.AddConsole(consoleLogOptions =>
            {
                // Configure all logs to go to stderr
                consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
            });

            builder.Services
                .AddMcpServer()
                .WithStdioServerTransport()
                .WithPromptsFromAssembly()
                .WithToolsFromAssembly();

            builder.Services
                .AddConnector()
                .AddLogging(loggingBuilder =>
                {
                    // loggingBuilder.AddSimpleConsole(options =>
                    // {
                    //     options.IncludeScopes = false;
                    //     options.SingleLine = true;
                    //     options.TimestampFormat = "hh:mm:ss ";
                    // });
                    // loggingBuilder.AddProvider(new ConsoleLoggerProvider());
                    loggingBuilder.AddConsole(consoleLogOptions =>
                    {
                        // Ensure logs are sent to stdout
                        consoleLogOptions.FormatterName = ConsoleFormatterNames.Systemd;
                    });
                    loggingBuilder.SetMinimumLevel(LogLevel.Information);
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
    }
}
#endif