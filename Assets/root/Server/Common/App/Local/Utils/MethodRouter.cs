#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Collections.Generic;
using com.IvanMurzak.Unity.MCP.Common.Data;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using R3;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public class MethodRouter : IMethodRouter
    {
        readonly ILogger<MethodRouter> _logger;
        readonly ILocalApp _localApp;
        readonly ILocalServer? _localServer;
        readonly IRemoteServer? _remoteServer;
        readonly CompositeDisposable _disposables = new();

        public MethodRouter(ILogger<MethodRouter> logger, ILocalApp localApp, IRemoteServer? remoteServer = null, ILocalServer? localServer = null)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogTrace("Ctor.");
            _localApp = localApp ?? throw new ArgumentNullException(nameof(localApp));
            _remoteServer = remoteServer;
            _localServer = localServer;
            if (localServer == null && remoteServer == null)
                throw new ArgumentNullException(nameof(remoteServer), "Either local or remote server must be set.");
        }

        public void SetConnection(HubConnection hubConnection)
        {
            _logger.LogTrace("SetConnection.");
            _disposables.Clear();

            SetServerConnection(hubConnection);
            SetLocalServerConnection(hubConnection);
        }

        void SetServerConnection(HubConnection hubConnection)
        {
            if (_remoteServer == null)
                return;

            hubConnection.On<IRequestCallTool>(Consts.RPC.RunCallTool, async message =>
                {
                    _logger.LogInformation("Call Tool called.");
                    var response = await _localApp.RunCallTool(message);
                    await _remoteServer.RespondOnCallTool(response);
                })
                .AddTo(_disposables);

            hubConnection.On<IRequestListTool>(Consts.RPC.RunListTool, async message =>
                {
                    _logger.LogInformation("List Tool called.");
                    var response = await _localApp.RunListTool(message);
                    await _remoteServer.RespondOnListTool(response);
                })
                .AddTo(_disposables);

            hubConnection.On<IRequestResourceContent>(Consts.RPC.RunResourceContent, async message =>
                {
                    _logger.LogInformation("Read Resource called.");
                    var response = await _localApp.RunResourceContent(message);
                    await _remoteServer.RespondOnResourceContent(response);
                })
                .AddTo(_disposables);

            hubConnection.On<IRequestListResources>(Consts.RPC.RunListResources, async message =>
                {
                    _logger.LogInformation("List Resources called.");
                    var response = await _localApp.RunListResources(message);
                    await _remoteServer.RespondOnListResources(response);
                })
                .AddTo(_disposables);

            hubConnection.On<IRequestListResourceTemplates>(Consts.RPC.RunListResourceTemplates, async message =>
                {
                    _logger.LogInformation("List Resource Templates called.");
                    var response = await _localApp.RunResourceTemplates(message);
                    await _remoteServer.RespondOnResourceTemplates(response);
                })
                .AddTo(_disposables);
        }

        void SetLocalServerConnection(HubConnection hubConnection)
        {
            if (_localServer == null)
                return;

            hubConnection.On<IResponseData<IResponseCallTool>>(Consts.RPC.ResponseCallTool, async message =>
                {
                    _logger.LogInformation("Response Call Tool called.");
                    await _localServer.RespondOnCallTool(message);
                })
                .AddTo(_disposables);

            hubConnection.On<IResponseData<List<IResponseListTool>>>(Consts.RPC.RunListTool, async message =>
                {
                    _logger.LogInformation("Response List Tool called.");
                    await _localServer.RespondOnListTool(message);
                })
                .AddTo(_disposables);

            hubConnection.On<IResponseData<List<IResponseResourceContent>>>(Consts.RPC.RunResourceContent, async message =>
                {
                    _logger.LogInformation("Response Read Resource called.");
                    await _localServer.RespondOnResourceContent(message);
                })
                .AddTo(_disposables);

            hubConnection.On<IResponseData<List<IResponseListResource>>>(Consts.RPC.RunListResources, async message =>
                {
                    _logger.LogInformation("Response List Resources called.");
                    await _localServer.RespondOnListResources(message);
                })
                .AddTo(_disposables);

            hubConnection.On<IResponseData<List<IResponseResourceTemplate>>>(Consts.RPC.RunListResourceTemplates, async message =>
                {
                    _logger.LogInformation("Response List Resource Templates called.");
                    await _localServer.RespondOnResourceTemplates(message);
                })
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}