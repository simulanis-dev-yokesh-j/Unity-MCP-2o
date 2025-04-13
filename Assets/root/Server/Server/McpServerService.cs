#if !IGNORE
using System;
using System.Threading;
using System.Threading.Tasks;
using com.IvanMurzak.Unity.MCP.Common;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using R3;

namespace com.IvanMurzak.Unity.MCP.Server
{
    public class McpServerService : IHostedService
    {
        readonly ILogger<McpServerService> _logger;
        readonly IMcpServer _mcpServer;
        readonly IMcpRunner _mcpRunner;
        readonly IRemoteApp _remoteApp;
        readonly ILocalServer _localServer;
        readonly CompositeDisposable _disposables = new();

        public IMcpRunner McpRunner => _mcpRunner;
        public IRemoteApp RemoteApp => _remoteApp;
        public ILocalServer LocalServer => _localServer;

        public static McpServerService? Instance { get; private set; }

        public McpServerService(ILogger<McpServerService> logger, IMcpServer mcpServer, IMcpRunner mcpRunner, IRemoteApp remoteApp, ILocalServer localServer)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogTrace("Ctor.");
            _mcpServer = mcpServer ?? throw new ArgumentNullException(nameof(mcpServer));
            _mcpRunner = mcpRunner ?? throw new ArgumentNullException(nameof(mcpRunner));
            _remoteApp = remoteApp ?? throw new ArgumentNullException(nameof(remoteApp));
            _localServer = localServer ?? throw new ArgumentNullException(nameof(localServer));

            if (Instance != null)
                throw new InvalidOperationException($"{typeof(McpServerService).Name} is already initialized.");
            Instance = this;

            LocalServer.OnListToolUpdated
                .Subscribe(_ =>
                {

                })
                .AddTo(_disposables);

            LocalServer.OnListResourcesUpdated
                .Subscribe(_ =>
                {

                })
                .AddTo(_disposables);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogTrace("StartAsync.");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogTrace("StopAsync.");
            _disposables.Clear();
            Instance = null;
            return McpPlugin.StaticDisposeAsync();
        }
    }
}
#endif