#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public interface IMcpAppBuilder
    {
        IServiceCollection Services { get; }
        IMcpAppBuilder AddTool(string name, IRunTool runner);
        IMcpAppBuilder AddResource(IRunResource resourceParams);
        IMcpAppBuilder AddLogging(Action<ILoggingBuilder> loggingBuilder);
        IMcpAppBuilder WithConfig(Action<ConnectionConfig> config);
        IMcpPlugin Build();
    }
}