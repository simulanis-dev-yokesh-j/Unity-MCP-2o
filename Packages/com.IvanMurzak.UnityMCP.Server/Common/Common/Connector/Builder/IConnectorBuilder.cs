using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.UnityMCP.Common.API
{
    public interface IConnectorBuilder
    {
        IServiceCollection Services { get; }
        IConnectorBuilder AddCommand(string fullPath, Command command);
        IConnectorBuilder AddLogging(Action<ILoggingBuilder> loggingBuilder);
        IConnectorBuilder WithConfig(Action<ConnectorConfig> config);
        IConnector Build();
    }
}