#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using com.IvanMurzak.Unity.MCP.Common.Data;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public class RemoteServer : IRemoteServer
    {
        protected readonly ILogger<RemoteServer> _logger;
        protected readonly IConnectionManager _connectionManager;

        public HubConnectionState ConnectionState => throw new NotImplementedException();

        public RemoteServer(ILogger<RemoteServer> logger, IConnectionManager connectionManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogTrace("Ctor.");
            _connectionManager = connectionManager ?? throw new ArgumentNullException(nameof(connectionManager));
        }

        public Task<IResponseData<string>> UpdateResources(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IResponseData<string>> UpdateTools(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IResponseData<string>> RespondOnCallTool(IResponseData<IResponseCallTool> data, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("RespondOnCallTool.");
            return _connectionManager.InvokeAsync<
                IResponseData<IResponseCallTool>,
                IResponseData<string>>(Consts.RPC.ResponseCallTool, data, cancellationToken);
        }
        public Task<IResponseData<string>> RespondOnListTool(IResponseData<List<IResponseListTool>> data, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("RespondOnListTool.");
            return _connectionManager.InvokeAsync<
                IResponseData<List<IResponseListTool>>,
                IResponseData<string>>(Consts.RPC.ResponseListTool, data, cancellationToken);
        }

        public Task<IResponseData<string>> RespondOnResourceContent(IResponseData<List<IResponseResourceContent>> data, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("RespondOnResourceContent.");
            return _connectionManager.InvokeAsync<
                IResponseData<List<IResponseResourceContent>>,
                IResponseData<string>>(Consts.RPC.ResponseResourceContent, data, cancellationToken);

        }
        public Task<IResponseData<string>> RespondOnListResources(IResponseData<List<IResponseListResource>> data, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("RespondOnListResources.");
            return _connectionManager.InvokeAsync<
                IResponseData<List<IResponseListResource>>,
                IResponseData<string>>(Consts.RPC.ResponseListResources, data, cancellationToken);
        }
        public Task<IResponseData<string>> RespondOnResourceTemplates(IResponseData<List<IResponseResourceTemplate>> data, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("RespondOnResourceTemplates.");
            return _connectionManager.InvokeAsync<
                IResponseData<List<IResponseResourceTemplate>>,
                IResponseData<string>>(Consts.RPC.ResponseListResourceTemplates, data, cancellationToken);
        }

        public Task<bool> Connect(CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Connect.");
            return _connectionManager.Connect(cancellationToken);
        }

        public Task Disconnect(CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Connect.");
            return _connectionManager.Disconnect(cancellationToken);
        }

        public void Dispose()
        {

        }
    }
}