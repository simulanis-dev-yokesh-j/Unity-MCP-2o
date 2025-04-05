#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.UnityMCP.Common.API
{
    public class ConnectorBuilder : IConnectorBuilder
    {
        readonly Dictionary<string, Command> commands = new();
        readonly IServiceCollection _services;

        public IServiceCollection Services => _services;

        public ConnectorBuilder(IServiceCollection? services = null)
        {
            _services = services ?? new ServiceCollection();
            _services.AddTransient<ICommandDispatcher, CommandDispatcher>();
            _services.AddTransient<IConnectorReceiver, Connector.Receiver>();
            _services.AddTransient<IConnectorSender, Connector.Sender>();
            _services.AddTransient<IConnector, Connector>();
            _services.AddSingleton(commands);
        }

        public IConnectorBuilder AddCommand(string fullPath, Command command)
        {
            if (commands.ContainsKey(fullPath))
                throw new ArgumentException($"Command with path {fullPath} already exists.");

            commands.Add(fullPath, command);
            return this;
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