#if !IGNORE
using System;
using System.Threading;
using System.Threading.Tasks;
using com.IvanMurzak.Unity.MCP.Common;
using Microsoft.Extensions.Hosting;

namespace com.IvanMurzak.Unity.MCP.Server
{
    public class McpServerService : IHostedService
    {
        readonly IMcpRunner _mcpRunner;
        readonly IRemoteApp _remoteApp;
        readonly ILocalServer _localServer;

        public IMcpRunner McpRunner => _mcpRunner;
        public IRemoteApp RemoteApp => _remoteApp;
        public ILocalServer LocalServer => _localServer;

        public static McpServerService? Instance { get; private set; }

        public McpServerService(IMcpRunner mcpRunner, IRemoteApp remoteApp, ILocalServer localServer)
        {
            _mcpRunner = mcpRunner ?? throw new ArgumentNullException(nameof(mcpRunner));
            _remoteApp = remoteApp ?? throw new ArgumentNullException(nameof(remoteApp));
            _localServer = localServer ?? throw new ArgumentNullException(nameof(localServer));

            if (Instance != null)
                throw new InvalidOperationException($"{typeof(McpServerService).Name} is already initialized.");
            Instance = this;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Force the creation of the IMcpApp instance
            // Any initialization logic can go here if needed
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Instance = null;
            McpPlugin.StaticDispose();
            return Task.CompletedTask;
        }
    }
}
#endif