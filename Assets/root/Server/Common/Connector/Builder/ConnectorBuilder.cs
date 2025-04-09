#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public class ConnectorBuilder : IConnectorBuilder
    {
        readonly IDictionary<string, IDictionary<string, IRunTool>> tools = new Dictionary<string, IDictionary<string, IRunTool>>();
        readonly IDictionary<string, IRunResource> resources = new Dictionary<string, IRunResource>();
        readonly IServiceCollection _services;

        public IServiceCollection Services => _services;

        public ConnectorBuilder(IServiceCollection? services = null)
        {
            _services = services ?? new ServiceCollection();
            _services.AddTransient<IResourceDispatcher, ResourceDispatcher>();
            _services.AddTransient<IToolDispatcher, ToolDispatcher>();
            _services.AddTransient<IConnectorReceiver, Connector.Receiver>();
            _services.AddTransient<IConnectorSender, Connector.Sender>();
            _services.AddTransient<IConnector, Connector>();
            _services.AddSingleton(tools);
            _services.AddSingleton(resources);
        }

        public IConnectorBuilder AddTool(string className, string method, RunTool command)
        {
            if (!tools.TryGetValue(className, out var commandGroup))
                tools[className] = commandGroup = new Dictionary<string, IRunTool>();

            if (commandGroup.ContainsKey(method))
                throw new ArgumentException($"Command with name '{method}' already exists in path {className}.");

            commandGroup.Add(method, command);
            return this;
        }

        public IConnectorBuilder AddResource(IRunResource resourceParams)
        {
            if (resources == null)
                throw new ArgumentNullException(nameof(resources));
            if (resourceParams == null)
                throw new ArgumentNullException(nameof(resourceParams));

            if (resources.ContainsKey(resourceParams.Route))
                throw new ArgumentException($"Resource with routing '{resourceParams.Route}' already exists.");

            resources.Add(resourceParams.Route, resourceParams);
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