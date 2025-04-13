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
    public class RpcRouter : IRpcRouter
    {
        readonly ILogger<RpcRouter> _logger;
        readonly IMcpRunner _localApp;
        readonly IRemoteServer _remoteServer;
        readonly IConnectionManager _connectionManager;
        readonly CompositeDisposable _serverEventsDisposables = new();
        readonly IDisposable _hubConnectionDisposable;

        public HubConnectionState ConnectionState => _connectionManager.ConnectionState;

        public RpcRouter(ILogger<RpcRouter> logger, IConnectionManager connectionManager, IMcpRunner localApp, IRemoteServer remoteServer)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogTrace("Ctor.");
            _localApp = localApp ?? throw new ArgumentNullException(nameof(localApp));
            _connectionManager = connectionManager ?? throw new ArgumentNullException(nameof(connectionManager));
            _remoteServer = remoteServer ?? throw new ArgumentNullException(nameof(remoteServer));

            _connectionManager.Endpoint = Consts.Hub.DefaultEndpoint + Consts.Hub.RemoteApp;

            _hubConnectionDisposable = connectionManager.HubConnection
                .Subscribe(SubscribeOnServerEvents);
        }

        public Task<bool> Connect(CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Connecting... (to RemoteApp: {0}).", _connectionManager.Endpoint);
            return _connectionManager.Connect(cancellationToken);
        }
        public Task Disconnect(CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Disconnecting... (to RemoteApp: {0}).", _connectionManager.Endpoint);
            return _connectionManager.Disconnect(cancellationToken);
        }

        void SubscribeOnServerEvents(HubConnection? hubConnection)
        {
            _logger.LogTrace("Clearing server events disposables.");
            _serverEventsDisposables.Clear();

            if (hubConnection == null)
                return;

            _logger.LogTrace("Subscribing to server events.");

            hubConnection.On<RequestCallTool, IResponseData<ResponseCallTool>>(Consts.RPC.RunCallTool, async data =>
                {
                    _logger.LogDebug("Call Tool called.");
                    return await _localApp.RunCallTool(data);
                })
                .AddTo(_serverEventsDisposables);

            hubConnection.On<RequestListTool, IResponseData<ResponseListTool[]>>(Consts.RPC.RunListTool, async data =>
                {
                    _logger.LogDebug("List Tool called.");
                    return await _localApp.RunListTool(data);
                })
                .AddTo(_serverEventsDisposables);

            hubConnection.On<RequestResourceContent, IResponseData<ResponseResourceContent[]>>(Consts.RPC.RunResourceContent, async data =>
                {
                    _logger.LogDebug("Read Resource content called.");
                    return await _localApp.RunResourceContent(data);
                })
                .AddTo(_serverEventsDisposables);

            hubConnection.On<RequestListResources, IResponseData<ResponseListResource[]>>(Consts.RPC.RunListResources, async data =>
                {
                    _logger.LogDebug("List Resources called.");
                    return await _localApp.RunListResources(data);
                })
                .AddTo(_serverEventsDisposables);

            hubConnection.On<RequestListResourceTemplates, IResponseData<ResponseResourceTemplate[]>>(Consts.RPC.RunListResourceTemplates, async data =>
                {
                    _logger.LogDebug("List Resource Templates called.");
                    return await _localApp.RunResourceTemplates(data);
                })
                .AddTo(_serverEventsDisposables);
        }

        // void SetLocalServerConnection(HubConnection hubConnection)
        // {
        //     if (_localServer == null)
        //         return;

        //     hubConnection.On<ResponseData<IResponseCallTool>>(Consts.RPC.ResponseCallTool, async message =>
        //         {
        //             _logger.LogInformation("Response Call Tool called.");
        //             await _localServer.RespondOnCallTool(message);
        //         })
        //         .AddTo(_serverEventsDisposables);

        //     hubConnection.On<ResponseData<List<IResponseListTool>>>(Consts.RPC.RunListTool, async message =>
        //         {
        //             _logger.LogInformation("Response List Tool called.");
        //             await _localServer.RespondOnListTool(message);
        //         })
        //         .AddTo(_serverEventsDisposables);

        //     hubConnection.On<ResponseData<List<IResponseResourceContent>>>(Consts.RPC.RunResourceContent, async message =>
        //         {
        //             _logger.LogInformation("Response Read Resource called.");
        //             await _localServer.RespondOnResourceContent(message);
        //         })
        //         .AddTo(_serverEventsDisposables);

        //     hubConnection.On<ResponseData<List<IResponseListResource>>>(Consts.RPC.RunListResources, async message =>
        //         {
        //             _logger.LogInformation("Response List Resources called.");
        //             await _localServer.RespondOnListResources(message);
        //         })
        //         .AddTo(_serverEventsDisposables);

        //     hubConnection.On<ResponseData<List<IResponseResourceTemplate>>>(Consts.RPC.RunListResourceTemplates, async message =>
        //         {
        //             _logger.LogInformation("Response List Resource Templates called.");
        //             await _localServer.RespondOnResourceTemplates(message);
        //         })
        //         .AddTo(_serverEventsDisposables);
        // }

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