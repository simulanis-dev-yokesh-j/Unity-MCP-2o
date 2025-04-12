#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using com.IvanMurzak.Unity.MCP.Common.Data;
using Microsoft.Extensions.Logging;
using R3;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public class LocalServer : ILocalServer
    {
        protected readonly ILogger<LocalServer> _logger;
        protected readonly IConnectionManager _connectionManager;

        readonly Subject<Unit> _onRespondCallTool = new Subject<Unit>();
        public Observable<Unit> OnRespondCallTool => _onRespondCallTool;

        readonly Subject<Unit> _onRespondListTool = new Subject<Unit>();
        public Observable<Unit> OnRespondListTool => _onRespondListTool;

        readonly Subject<Unit> _onRespondResourceContent = new Subject<Unit>();
        public Observable<Unit> OnRespondResourceContent => _onRespondResourceContent;

        readonly Subject<Unit> _onRespondListResources = new Subject<Unit>();
        public Observable<Unit> OnRespondListResources => _onRespondListResources;

        readonly Subject<Unit> _onRespondResourceTemplates = new Subject<Unit>();
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

        public Task RespondOnResourceTemplates(IResponseData<List<IResponseResourceTemplate>> data, CancellationToken cancellationToken = default)
        {
            _onRespondResourceTemplates.OnNext(Unit.Default);
            return Task.CompletedTask;
        }

        public void Dispose()
        {

        }
    }
}