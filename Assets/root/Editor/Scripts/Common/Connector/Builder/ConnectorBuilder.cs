using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.UnityMCP.Common.API
{
    public class ConnectorBuilder : IConnectorBuilder
    {
        internal List<Action<IConnector>> buildActions = new();

        readonly IServiceCollection _services;

        public ConnectorBuilder(IServiceCollection? services = null)
        {
            _services = services ?? new ServiceCollection();
            _services.AddTransient<IConnector, Connector>();
        }

        public IConnectorBuilder AddLogging(Action<ILoggingBuilder> loggingBuilder)
        {
            _services.AddLogging(loggingBuilder);
            return this;
        }

        public IConnectorBuilder WithConfig(Action<ConnectorConfig> config)
        {
            _services.Configure(config);
            return this;
        }

        public IConnector Build() => _services
            .BuildServiceProvider()
            .GetRequiredService<IConnector>();
    }
}