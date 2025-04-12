#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Common.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using R3;

namespace com.IvanMurzak.Unity.MCP.Server
{
    public class LocalServer : Hub, ILocalServer
    {
        protected readonly ILogger<LocalServer> _logger;
        protected readonly IConnectionManager _connectionManager;

        readonly Subject<Unit> _onRespondCallTool = new();
        public Observable<Unit> OnRespondCallTool => _onRespondCallTool;

        readonly Subject<Unit> _onRespondListTool = new();
        public Observable<Unit> OnRespondListTool => _onRespondListTool;

        readonly Subject<Unit> _onRespondResourceContent = new();
        public Observable<Unit> OnRespondResourceContent => _onRespondResourceContent;

        readonly Subject<Unit> _onRespondListResources = new();
        public Observable<Unit> OnRespondListResources => _onRespondListResources;

        readonly Subject<Unit> _onRespondResourceTemplates = new();
        public Observable<Unit> OnRespondResourceTemplates => _onRespondResourceTemplates;

        public LocalServer(ILogger<LocalServer> logger, IConnectionManager connectionManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogTrace("Ctor.");
            _connectionManager = connectionManager ?? throw new ArgumentNullException(nameof(connectionManager));
        }

        public Task RespondOnCallTool(IResponseData<IResponseCallTool> data, CancellationToken cancellationToken = default)
        {
            _onRespondCallTool.OnNext(Unit.Default);
            return Task.CompletedTask;
        }

        public Task RespondOnListTool(IResponseData<List<IResponseListTool>> data, CancellationToken cancellationToken = default)
        {
            _onRespondListTool.OnNext(Unit.Default);
            return Task.CompletedTask;
        }

        public Task RespondOnResourceContent(IResponseData<List<IResponseResourceContent>> data, CancellationToken cancellationToken = default)
        {
            _onRespondResourceContent.OnNext(Unit.Default);
            return Task.CompletedTask;
        }

        public Task RespondOnListResources(IResponseData<List<IResponseListResource>> data, CancellationToken cancellationToken = default)
        {
            _onRespondListResources.OnNext(Unit.Default);
            return Task.CompletedTask;
        }

        public Task RespondOnListResourceTemplates(IResponseData<List<IResponseResourceTemplate>> data, CancellationToken cancellationToken = default)
        {
            _onRespondResourceTemplates.OnNext(Unit.Default);
            return Task.CompletedTask;
        }

        public new void Dispose()
        {
            base.Dispose();
        }
    }
}