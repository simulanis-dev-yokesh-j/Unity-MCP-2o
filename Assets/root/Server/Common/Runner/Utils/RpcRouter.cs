#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Collections.Generic;
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
            _logger.LogTrace("Connect.");
            return _connectionManager.Connect(cancellationToken);
        }
        public Task Disconnect(CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Disconnect.");
            return _connectionManager.Disconnect(cancellationToken);
        }

        void SubscribeOnServerEvents(HubConnection hubConnection)
        {
            _serverEventsDisposables.Clear();

            hubConnection.On<IRequestCallTool>(Consts.RPC.RunCallTool, async message =>
                {
                    _logger.LogInformation("Call Tool called.");
                    var response = await _localApp.RunCallTool(message);
                    await _remoteServer.RespondOnCallTool(response);
                })
                .AddTo(_serverEventsDisposables);

            hubConnection.On<IRequestListTool>(Consts.RPC.RunListTool, async message =>
                {
                    _logger.LogInformation("List Tool called.");
                    var response = await _localApp.RunListTool(message);
                    await _remoteServer.RespondOnListTool(response);
                })
                .AddTo(_serverEventsDisposables);

            hubConnection.On<IRequestResourceContent>(Consts.RPC.RunResourceContent, async message =>
                {
                    _logger.LogInformation("Read Resource called.");
                    var response = await _localApp.RunResourceContent(message);
                    await _remoteServer.RespondOnResourceContent(response);
                })
                .AddTo(_serverEventsDisposables);

            hubConnection.On<IRequestListResources>(Consts.RPC.RunListResources, async message =>
                {
                    _logger.LogInformation("List Resources called.");
                    var response = await _localApp.RunListResources(message);
                    await _remoteServer.RespondOnListResources(response);
                })
                .AddTo(_serverEventsDisposables);

            hubConnection.On<IRequestListResourceTemplates>(Consts.RPC.RunListResourceTemplates, async message =>
                {
                    _logger.LogInformation("List Resource Templates called.");
                    var response = await _localApp.RunResourceTemplates(message);
                    await _remoteServer.RespondOnResourceTemplates(response);
                })
                .AddTo(_serverEventsDisposables);
        }

        // void SetLocalServerConnection(HubConnection hubConnection)
        // {
        //     if (_localServer == null)
        //         return;

        //     hubConnection.On<IResponseData<IResponseCallTool>>(Consts.RPC.ResponseCallTool, async message =>
        //         {
        //             _logger.LogInformation("Response Call Tool called.");
        //             await _localServer.RespondOnCallTool(message);
        //         })
        //         .AddTo(_serverEventsDisposables);

        //     hubConnection.On<IResponseData<List<IResponseListTool>>>(Consts.RPC.RunListTool, async message =>
        //         {
        //             _logger.LogInformation("Response List Tool called.");
        //             await _localServer.RespondOnListTool(message);
        //         })
        //         .AddTo(_serverEventsDisposables);

        //     hubConnection.On<IResponseData<List<IResponseResourceContent>>>(Consts.RPC.RunResourceContent, async message =>
        //         {
        //             _logger.LogInformation("Response Read Resource called.");
        //             await _localServer.RespondOnResourceContent(message);
        //         })
        //         .AddTo(_serverEventsDisposables);

        //     hubConnection.On<IResponseData<List<IResponseListResource>>>(Consts.RPC.RunListResources, async message =>
        //         {
        //             _logger.LogInformation("Response List Resources called.");
        //             await _localServer.RespondOnListResources(message);
        //         })
        //         .AddTo(_serverEventsDisposables);

        //     hubConnection.On<IResponseData<List<IResponseResourceTemplate>>>(Consts.RPC.RunListResourceTemplates, async message =>
        //         {
        //             _logger.LogInformation("Response List Resource Templates called.");
        //             await _localServer.RespondOnResourceTemplates(message);
        //         })
        //         .AddTo(_serverEventsDisposables);
        // }

        public void Dispose()
        {
            _serverEventsDisposables.Dispose();
            _hubConnectionDisposable.Dispose();
        }
    }
}