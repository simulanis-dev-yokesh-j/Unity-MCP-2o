#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public class ConnectorBuilder : IConnectorBuilder
    {
        readonly IDictionary<string, IRunTool> tools = new Dictionary<string, IRunTool>();
        readonly IDictionary<string, IRunResource> resources = new Dictionary<string, IRunResource>();
        readonly IServiceCollection _services;

        public IServiceCollection Services => _services;

        public ConnectorBuilder(IServiceCollection? services = null)
        {
            _services = services ?? new ServiceCollection();

            // _services.AddOptions<HttpConnectionOptions>()
            //     .Configure(options =>
            //     {
            //         options.Transports = HttpTransportType.WebSockets | HttpTransportType.ServerSentEvents;
            //         options.Url = new Uri("http://localhost:60606/connector");
            //         options.SkipNegotiation = false;
            //         options.WebSocketConfiguration = wsOptions =>
            //         {
            //             wsOptions.KeepAliveInterval = TimeSpan.FromSeconds(30);
            //         };
            //         // options.WebSockets.ClientWebSocketOptions.KeepAliveInterval = TimeSpan.FromSeconds(30);
            //     });
            // _services.AddTransient<IConnectionFactory, HttpConnectionFactory>();
            // _services.Add

            // _services.AddSingleton(new HubConnectionBuilder()
            //     .WithUrl("http://localhost:60606/connector") // TODO: add reading from configs (json file and env variables)
            //     .WithAutomaticReconnect());

            _services.AddTransient<ILocalApp, LocalApp>();
            _services.AddTransient<IConnector, Connector>();

            _services.AddSingleton(tools);
            _services.AddSingleton(resources);

            _services.AddSingleton<Func<Task<HubConnection>>>(() =>
            {
                return new HubConnectionBuilder()
                    .WithUrl("http://localhost:60606/connector") // TODO: add reading from configs (json file and env variables)
                    .WithAutomaticReconnect()
                    .AddJsonProtocol(options =>
                    {
                        options.PayloadSerializerOptions.PropertyNamingPolicy = null;
                        options.PayloadSerializerOptions.DictionaryKeyPolicy = null;
                    })
                    .Build()
                    .TaskFromResult();
            });
        }

        public IConnectorBuilder AddTool(string name, IRunTool runner)
        {
            if (tools.ContainsKey(name))
                throw new ArgumentException($"Tool with name '{name}' already exists.");

            tools.Add(name, runner);
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