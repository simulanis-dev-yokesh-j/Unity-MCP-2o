#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using R3;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public partial class McpPlugin : IMcpPlugin
    {
        public const string Version = "0.1.0";

        readonly ILogger<McpPlugin> _logger;
        readonly IRpcRouter _rpcRouter;
        readonly Func<Task<HubConnection>> _hubConnectionBuilder;

        HubConnection? hubConnection;

        public IMcpRunner McpRunner { get; private set; }
        public IRemoteApp? RemoteApp { get; private set; } = null;
        public IRemoteServer? RemoteServer { get; private set; } = null;
        public HubConnectionState GetStatus => hubConnection?.State ?? HubConnectionState.Disconnected;

        // IOptions<ConnectorConfig> configOptions
        public McpPlugin(ILogger<McpPlugin> logger, Func<Task<HubConnection>> hubConnectionBuilder, IRpcRouter rpcRouter, IMcpRunner mcpRunner, IRemoteApp? app = null, IRemoteServer? remoteServer = null)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogTrace("Ctor. Version: {0}", Version);

            _hubConnectionBuilder = hubConnectionBuilder ?? throw new ArgumentNullException(nameof(hubConnectionBuilder));
            _rpcRouter = rpcRouter ?? throw new ArgumentNullException(nameof(rpcRouter));

            McpRunner = mcpRunner ?? throw new ArgumentNullException(nameof(mcpRunner));
            RemoteApp = app;
            RemoteServer = remoteServer;

            if (HasInstance)
            {
                _logger.LogError("Connector already created. Use Singleton instance.");
                return;
            }

            instance = this;
        }

        public async Task Connect()
        {
            if (hubConnection == null)
            {
                hubConnection = await _hubConnectionBuilder();
                if (hubConnection == null)
                {
                    _logger.LogError("Can't establish connection with Remote.");
                    return;
                }

                _rpcRouter.SetConnection(hubConnection);
            }

            if (hubConnection.State == HubConnectionState.Connected ||
                hubConnection.State == HubConnectionState.Reconnecting)
                return;

            await hubConnection.StartAsync();
        }

        public void Disconnect()
        {
            if (hubConnection == null)
                return;

            hubConnection.StopAsync().Wait();
        }

        public void Dispose()
        {
            if (hubConnection == null)
                return;

            hubConnection.StopAsync().Wait();
            hubConnection.DisposeAsync().AsTask().Wait();
            instance = null;
        }
        ~McpPlugin() => Dispose();
    }
}