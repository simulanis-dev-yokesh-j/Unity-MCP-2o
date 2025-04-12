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
            _connectionManager.Endpoint = Consts.Hub.DefaultEndpoint + Consts.Hub.LocalServer;
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
                IResponseData<string>>(Consts.RPC.ResponseOnCallTool, data, cancellationToken);
        }
        public Task<IResponseData<string>> RespondOnListTool(IResponseData<List<IResponseListTool>> data, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("RespondOnListTool.");
            return _connectionManager.InvokeAsync<
                IResponseData<List<IResponseListTool>>,
                IResponseData<string>>(Consts.RPC.ResponseOnListTool, data, cancellationToken);
        }

        public Task<IResponseData<string>> RespondOnResourceContent(IResponseData<List<IResponseResourceContent>> data, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("RespondOnResourceContent.");
            return _connectionManager.InvokeAsync<
                IResponseData<List<IResponseResourceContent>>,
                IResponseData<string>>(Consts.RPC.ResponseOnResourceContent, data, cancellationToken);
        }
        public Task<IResponseData<string>> RespondOnListResources(IResponseData<List<IResponseListResource>> data, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("RespondOnListResources.");
            return _connectionManager.InvokeAsync<
                IResponseData<List<IResponseListResource>>,
                IResponseData<string>>(Consts.RPC.ResponseOnListResources, data, cancellationToken);
        }
        public Task<IResponseData<string>> RespondOnResourceTemplates(IResponseData<List<IResponseResourceTemplate>> data, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("RespondOnResourceTemplates.");
            return _connectionManager.InvokeAsync<
                IResponseData<List<IResponseResourceTemplate>>,
                IResponseData<string>>(Consts.RPC.ResponseOnListResourceTemplates, data, cancellationToken);
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

        }
    }
}