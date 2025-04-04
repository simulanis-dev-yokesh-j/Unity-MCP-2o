using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.UnityMCP.Common.API
{
    public class ConnectorBuilder : IConnectorBuilder
    {
        internal List<Action<IConnector>> buildActions = new();

        readonly ServiceCollection _services;

        public ConnectorBuilder()
        {
            _services = new ServiceCollection();
            _services.AddTransient<IConnector, Connector>();
        }

        public IConnectorBuilder AddLogging(Action<ILoggingBuilder> loggingBuilder)
        {
            _services.AddLogging(loggingBuilder);
            return this;
        }

        public IConnector Build() => _services
            .BuildServiceProvider()
            .GetRequiredService<IConnector>();

        public IConnector TEST()
        {
            var services = new ServiceCollection();

            // Register ConnectorConfig with default values or from configuration
            services.Configure<ConnectorConfig>(config =>
            {
                config.Hostname = "127.0.0.1"; // Replace with actual hostname
                config.Port = 60606;          // Replace with actual port
            });

            // Register ILogger (if not already registered)
            services.AddLogging();

            // Register Connector
            services.AddTransient<IConnector, Connector>();

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetRequiredService<IConnector>();
        }
    }
}