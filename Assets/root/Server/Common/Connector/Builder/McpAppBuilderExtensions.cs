#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using Microsoft.Extensions.DependencyInjection;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public static partial class McpAppBuilderExtensions
    {
        public static IMcpAppBuilder AddMcpApp(this IServiceCollection services)
            => new McpAppBuilder(services);

        public static IMcpAppBuilder AddRemoteApp(this IMcpAppBuilder builder)
        {
            builder.Services.AddTransient<IRemoteApp, RemoteApp>();
            return builder;
        }
        public static IMcpAppBuilder AddLocalApp(this IMcpAppBuilder builder)
        {
            builder.Services.AddTransient<ILocalApp, LocalApp>();
            return builder;
        }
        public static IMcpAppBuilder AddRemoteServer(this IMcpAppBuilder builder)
        {
            builder.Services.AddTransient<IRemoteServer, RemoteServer>();
            return builder;
        }
    }
}