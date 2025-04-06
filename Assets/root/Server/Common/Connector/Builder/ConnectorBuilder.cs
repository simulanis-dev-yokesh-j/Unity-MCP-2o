#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public class ConnectorBuilder : IConnectorBuilder
    {
        readonly IDictionary<string, IDictionary<string, ICommand>> commands = new Dictionary<string, IDictionary<string, ICommand>>();
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

        public IConnectorBuilder AddCommand(string className, string method, Command command)
        {
            if (!commands.TryGetValue(className, out var commandGroup))
                commands[className] = commandGroup = new Dictionary<string, ICommand>();

            if (commandGroup.ContainsKey(method))
                throw new ArgumentException($"Command with name {method} already exists in path {className}.");

            commandGroup.Add(method, command);
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