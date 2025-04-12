#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public static partial class McpAppBuilderExtensions
    {
        public static IServiceCollection AddMcpApp(this IServiceCollection services, Action<IMcpAppBuilder>? configure = null)
        {
            // Create an instance of McpAppBuilder
            var mcpAppBuilder = new McpAppBuilder(services);

            // Allow additional configuration of McpAppBuilder
            configure?.Invoke(mcpAppBuilder);

            return services;
        }
        public static IMcpAppBuilder WithAppFeatures(this IMcpAppBuilder builder)
        {
            builder.AddLocalApp();
            builder.AddRemoteServer();

            // TODO: Oncomment if any tools or prompts are needed from this assembly
            // var assembly = typeof(McpAppBuilderExtensions).Assembly;

            // builder.WithToolsFromAssembly(assembly);
            // builder.WithPromptsFromAssembly(assembly);
            // builder.WithResourcesFromAssembly(assembly);

            return builder;
        }

        public static IMcpAppBuilder WithServerFeatures(this IMcpAppBuilder builder)
        {
            builder.AddLocalApp();
            builder.AddLocalServer();
            builder.AddRemoteApp();
            return builder;
        }

        public static IMcpAppBuilder AddLocalApp(this IMcpAppBuilder builder)
        {
            builder.Services.TryAddSingleton<ILocalApp, LocalApp>();
            return builder;
        }
        public static IMcpAppBuilder AddLocalServer(this IMcpAppBuilder builder)
        {
            builder.Services.TryAddSingleton<ILocalServer, LocalServer>();
            return builder;
        }
        public static IMcpAppBuilder AddRemoteApp(this IMcpAppBuilder builder)
        {
            builder.Services.TryAddSingleton<IRemoteApp, RemoteApp>();
            return builder;
        }
        public static IMcpAppBuilder AddRemoteServer(this IMcpAppBuilder builder)
        {
            builder.Services.TryAddSingleton<IRemoteServer, RemoteServer>();
            return builder;
        }
    }
}