#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
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

        public Task<IResponseData<string>> NotifyAboutUpdatedTools(CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Notify server about updated tools.");
            return _connectionManager.InvokeAsync<string, IResponseData<string>>(Consts.RPC.Server.SetOnListToolsUpdated, string.Empty, cancellationToken);
        }

        public Task<IResponseData<string>> NotifyAboutUpdatedResources(CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Notify server about updated resources.");
            return _connectionManager.InvokeAsync<string, IResponseData<string>>(Consts.RPC.Server.SetOnListResourcesUpdated, string.Empty, cancellationToken);
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
            return _connectionManager.DisposeAsync();
        }
    }
}