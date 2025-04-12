#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using R3;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public partial class McpApp : IMcpApp
    {
        public const string Version = "0.1.0";

        readonly ILogger<McpApp> _logger;
        readonly IRpcRouter _methodRouter;
        readonly Func<Task<HubConnection>> _hubConnectionBuilder;

        HubConnection? hubConnection;

        public IRemoteServer? RemoteServer { get; private set; } = null;
        public IRemoteApp? RemoteApp { get; private set; } = null;
        public ILocalApp LocalApp { get; private set; }
        public HubConnectionState GetStatus => hubConnection?.State ?? HubConnectionState.Disconnected;

        // IOptions<ConnectorConfig> configOptions
        public McpApp(ILogger<McpApp> logger, Func<Task<HubConnection>> hubConnectionBuilder, IRpcRouter methodRouter, ILocalApp appLocal, IRemoteApp? app = null, IRemoteServer? server = null)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogTrace("Ctor. Version: {0}", Version);

            _hubConnectionBuilder = hubConnectionBuilder ?? throw new ArgumentNullException(nameof(hubConnectionBuilder));
            _methodRouter = methodRouter ?? throw new ArgumentNullException(nameof(methodRouter));

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

        public async Task Connect()
        {
            if (hubConnection == null)
            {
                hubConnection = await _hubConnectionBuilder();
                if (hubConnection == null)
                {
                    _logger.LogError("Can't establish connection with Remote.");
                    return;
                }

                _methodRouter.SetConnection(hubConnection);
            }

            if (hubConnection.State == HubConnectionState.Connected ||
                hubConnection.State == HubConnectionState.Reconnecting)
                return;

            await hubConnection.StartAsync();
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
            instance = null;
        }
        ~McpApp() => Dispose();
    }
}