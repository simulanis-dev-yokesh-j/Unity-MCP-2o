#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using R3;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public class McpAppBuilder : IMcpAppBuilder
    {
        readonly IServiceCollection _services;
        readonly IDictionary<string, IRunTool> _tools = new Dictionary<string, IRunTool>();
        readonly IDictionary<string, IRunResource> _resources = new Dictionary<string, IRunResource>();

        public IServiceCollection Services => _services;

        public McpAppBuilder(IServiceCollection? services = null)
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
            _services.AddTransient<IMcpApp, McpApp>();

            _services.AddSingleton(_tools);
            _services.AddSingleton(_resources);

            // new ReactiveProperty<HubConnection>(null)
            //     .Subscribe(hubConnection =>
            //     {
            //         if (hubConnection == null)
            //             return;

            //         hubConnection.On<IRequestListTool>(Consts.RCP.RunListTool, message => { });
            //     });

            _services.AddSingleton<Func<HubConnection>>(() =>
            {
                var hubConnection = new HubConnectionBuilder()
                    .WithUrl("http://localhost:60606/connector") // TODO: add reading from configs (json file and env variables)
                    .WithAutomaticReconnect()
                    .AddJsonProtocol(options =>
                    {
                        options.PayloadSerializerOptions.PropertyNamingPolicy = null;
                        options.PayloadSerializerOptions.DictionaryKeyPolicy = null;
                    })
                    .Build();

                return hubConnection;
            });
        }

        public IMcpAppBuilder AddTool(string name, IRunTool runner)
        {
            if (_tools.ContainsKey(name))
                throw new ArgumentException($"Tool with name '{name}' already exists.");

            _tools.Add(name, runner);
            return this;
        }

        public IMcpAppBuilder AddResource(IRunResource resourceParams)
        {
            if (_resources == null)
                throw new ArgumentNullException(nameof(_resources));
            if (resourceParams == null)
                throw new ArgumentNullException(nameof(resourceParams));

            if (_resources.ContainsKey(resourceParams.Route))
                throw new ArgumentException($"Resource with routing '{resourceParams.Route}' already exists.");

            _resources.Add(resourceParams.Route, resourceParams);
            return this;
        }

        public IMcpAppBuilder AddLogging(Action<ILoggingBuilder> loggingBuilder)
        {
            _services.AddLogging(loggingBuilder);
            return this;
        }

        public IMcpAppBuilder WithConfig(Action<ConnectorConfig> config)
        {
            _services.Configure(config);
            return this;
        }

        public IMcpApp Build() => _services
            .BuildServiceProvider()
            .GetRequiredService<IMcpApp>();
    }
}