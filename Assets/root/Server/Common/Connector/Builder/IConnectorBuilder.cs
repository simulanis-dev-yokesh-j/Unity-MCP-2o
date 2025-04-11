#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public interface IConnectorBuilder
    {
        IServiceCollection Services { get; }
        IConnectorBuilder AddTool(string className, string method, RunTool runner);
        IConnectorBuilder AddResource(IRunResource resourceParams);
        IConnectorBuilder AddLogging(Action<ILoggingBuilder> loggingBuilder);
        IConnectorBuilder WithConfig(Action<ConnectorConfig> config);
        IConnector Build();
    }
}