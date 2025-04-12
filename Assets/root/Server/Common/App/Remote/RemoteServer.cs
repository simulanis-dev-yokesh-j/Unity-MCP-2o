#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using com.IvanMurzak.Unity.MCP.Common.Data;
using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public class RemoteServer : IRemoteServer
    {
        protected readonly ILogger<RemoteServer> _logger;
        protected readonly IConnectionManager _connectionManager;

        public RemoteServer(ILogger<RemoteServer> logger, IConnectionManager connectionManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogTrace("Ctor.");
            _connectionManager = connectionManager ?? throw new ArgumentNullException(nameof(connectionManager));
        }

        public Task UpdateResources(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateTools(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task RespondOnCallTool(IResponseData<IResponseCallTool> data, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("RespondOnCallTool.");
            return _connectionManager.InvokeAsync(Consts.RPC.ResponseCallTool, data, cancellationToken);
        }
        public Task RespondOnListTool(IResponseData<List<IResponseListTool>> data, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("RespondOnListTool.");
            return _connectionManager.InvokeAsync(Consts.RPC.ResponseListTool, data, cancellationToken);
        }

        public Task RespondOnResourceContent(IResponseData<List<IResponseResourceContent>> data, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("RespondOnResourceContent.");
            return _connectionManager.InvokeAsync(Consts.RPC.ResponseResourceContent, data, cancellationToken);

        }
        public Task RespondOnListResources(IResponseData<List<IResponseListResource>> data, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("RespondOnListResources.");
            return _connectionManager.InvokeAsync(Consts.RPC.ResponseListResources, data, cancellationToken);
        }
        public Task RespondOnResourceTemplates(IResponseData<List<IResponseResourceTemplate>> data, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("RespondOnResourceTemplates.");
            return _connectionManager.InvokeAsync(Consts.RPC.ResponseListResourceTemplates, data, cancellationToken);
        }

        public void Dispose()
        {

        }
    }
}