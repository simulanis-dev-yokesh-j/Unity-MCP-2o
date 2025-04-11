#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public partial class Connector : IConnector
    {
        public const string Version = "0.1.0";

        readonly ILogger<Connector> _logger;
        readonly IHubConnectionBuilder _hubBuilder;

        HubConnection? hubConnection;

        public IRemoteServer? RemoteServer { get; private set; } = null;
        public IRemoteApp? RemoteApp { get; private set; } = null;
        public ILocalApp LocalApp { get; private set; }
        public HubConnectionState GetStatus => hubConnection?.State ?? HubConnectionState.Disconnected;

        // IOptions<ConnectorConfig> configOptions
        public Connector(ILogger<Connector> logger, IHubConnectionBuilder hubBuilder, ILocalApp appLocal, IRemoteApp? app = null, IRemoteServer? server = null)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogTrace("Ctor. Version: {0}", Version);

            _hubBuilder = hubBuilder ?? throw new ArgumentNullException(nameof(hubBuilder));

            LocalApp = appLocal ?? throw new ArgumentNullException(nameof(appLocal));
            RemoteApp = app;
            RemoteServer = server;

            if (HasInstance)
            {
                _logger.LogError("Connector already created. Use Singleton instance.");
                return;
            }

            instance = this;
        }

        public void Connect()
        {
            hubConnection ??= _hubBuilder.Build();
        }

        public void Disconnect()
        {
            if (hubConnection == null)
                return;

            hubConnection.StopAsync().Wait();
        }

        public void Dispose()
        {
            if (hubConnection == null)
                return;

            hubConnection.StopAsync().Wait();
            hubConnection.DisposeAsync().AsTask().Wait();
        }
        ~Connector() => Dispose();
    }
}