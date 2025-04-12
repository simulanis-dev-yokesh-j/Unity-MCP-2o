using com.IvanMurzak.Unity.MCP.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace com.IvanMurzak.Unity.MCP.Server
{
    public static class McpServerBuilderExtensions
    {
        public static IMcpPluginBuilder WithServerFeatures(this IMcpPluginBuilder builder)
        {
            builder.Services.AddRouting();
            builder.Services.AddHostedService<McpServerService>();
            builder.AddMcpRunner();
            builder.AddLocalServer();
            builder.AddRemoteApp();
            return builder;
        }
        public static IMcpPluginBuilder AddRemoteApp(this IMcpPluginBuilder builder)
        {
            builder.Services.TryAddSingleton<IRemoteApp, RemoteApp>();
            return builder;
        }
        public static IMcpPluginBuilder AddLocalServer(this IMcpPluginBuilder builder)
        {
            builder.Services.TryAddSingleton<ILocalServer, LocalServer>();
            return builder;
        }
    }
}