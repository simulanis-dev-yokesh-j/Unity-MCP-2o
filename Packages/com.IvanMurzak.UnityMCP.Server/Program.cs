using com.IvanMurzak.UnityMCP.Common.API;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace com.IvanMurzak.UnityMCP.Server
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
                    loggingBuilder.AddSimpleConsole(options =>
                    {
                        options.IncludeScopes = false;
                        options.SingleLine = true;
                        options.TimestampFormat = "hh:mm:ss ";
                    });
                    loggingBuilder.AddProvider(new ConsoleLoggerProvider());
                    // loggingBuilder.AddConsole(consoleLogOptions =>
                    // {
                    //     // Ensure logs are sent to stdout
                    //     consoleLogOptions.FormatterName = ConsoleFormatterNames.Systemd;
                    // });
                    loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                })
                .WithConfig(config =>
                {
                    config.ConnectionType = Connector.ConnectionType.Server;
                })
                .Build()
                .Connect();

            await builder.Build().RunAsync();
        }
    }
}