#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public interface IConnectorBuilder
    {
        IServiceCollection Services { get; }
        IConnectorBuilder AddCommand(string className, string method, Command command);
        IConnectorBuilder AddResource(string routing, Command command);
        IConnectorBuilder AddLogging(Action<ILoggingBuilder> loggingBuilder);
        IConnectorBuilder WithConfig(Action<ConnectorConfig> config);
        IConnector Build();
    }
}