#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Threading;
using System.Threading.Tasks;
using com.IvanMurzak.Unity.MCP.Common.Data;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using R3;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public class RemoteServer : IRemoteServer
    {
        readonly ILogger<RemoteServer> _logger;
        readonly IConnectionManager _connectionManager;
        readonly CompositeDisposable _serverEventsDisposables = new();
        readonly IDisposable _hubConnectionDisposable;
        readonly string guid = System.Guid.NewGuid().ToString();

        public ReadOnlyReactiveProperty<HubConnectionState> ConnectionState => throw new NotImplementedException();
        public ReadOnlyReactiveProperty<bool> KeepConnected => throw new NotImplementedException();

        public RemoteServer(ILogger<RemoteServer> logger, IConnectionManager connectionManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogTrace("Ctor.");
            _connectionManager = connectionManager ?? throw new ArgumentNullException(nameof(connectionManager));
            _connectionManager.Endpoint = Consts.Hub.DefaultEndpoint + Consts.Hub.LocalServer;

            _hubConnectionDisposable = connectionManager.HubConnection
                .Subscribe(SubscribeOnServerEvents);
        }

        void SubscribeOnServerEvents(HubConnection? hubConnection)
        {
            _logger.LogTrace("Clearing server events disposables.");
            _serverEventsDisposables.Clear();

            if (hubConnection == null)
                return;

            _logger.LogTrace("Subscribing to server events.");

            // hubConnection.On<string, string>(Consts.Hub.Ping,
            //     ping => ping == Consts.Hub.Ping
            //         ? Consts.Hub.Pong
            //         : ping)
            //     .AddTo(_serverEventsDisposables);

            hubConnection.On<string, string>(Consts.Hub.Ping,
                ping =>
                {
                    _logger.LogTrace("ping " + guid);
                    return ping == Consts.Hub.Ping
                        ? Consts.Hub.Pong
                        : ping;
                })
                .AddTo(_serverEventsDisposables);
        }

        public Task<ResponseData<string>> NotifyAboutUpdatedTools(CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Notify server about updated tools.");
            return _connectionManager.InvokeAsync<string, ResponseData<string>>(Consts.RPC.Server.SetOnListToolsUpdated, string.Empty, cancellationToken);
        }

        public Task<ResponseData<string>> NotifyAboutUpdatedResources(CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Notify server about updated resources.");
            return _connectionManager.InvokeAsync<string, ResponseData<string>>(Consts.RPC.Server.SetOnListResourcesUpdated, string.Empty, cancellationToken);
        }

        public Task<bool> Connect(CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Connecting... (to LocalServer: {0}).", _connectionManager.Endpoint);
            return _connectionManager.Connect(cancellationToken);
        }

        public Task Disconnect(CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Disconnecting... (from LocalServer: {0}).", _connectionManager.Endpoint);
            return _connectionManager.Disconnect(cancellationToken);
        }

        public void Dispose()
        {
            DisposeAsync().Wait();
        }

        public Task DisposeAsync()
        {
            _serverEventsDisposables.Dispose();
            _hubConnectionDisposable.Dispose();

            return _connectionManager.DisposeAsync();
        }
    }
}