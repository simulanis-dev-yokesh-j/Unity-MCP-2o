#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using Microsoft.Extensions.DependencyInjection;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public static partial class ConnectorBuilderExtensions
    {
        public static IConnectorBuilder AddConnector(this IServiceCollection services)
            => new ConnectorBuilder(services);

        public static IConnectorBuilder AddRemoteApp(this IConnectorBuilder builder)
        {
            builder.Services.AddTransient<IRemoteApp, RemoteApp>();
            return builder;
        }
        public static IConnectorBuilder AddLocalApp(this IConnectorBuilder builder)
        {
            builder.Services.AddTransient<ILocalApp, LocalApp>();
            return builder;
        }
        public static IConnectorBuilder AddRemoteServer(this IConnectorBuilder builder)
        {
            builder.Services.AddTransient<IRemoteServer, RemoteServer>();
            return builder;
        }
    }
}