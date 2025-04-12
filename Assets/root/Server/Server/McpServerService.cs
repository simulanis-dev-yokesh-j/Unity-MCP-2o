#if !IGNORE
using System;
using System.Threading;
using System.Threading.Tasks;
using com.IvanMurzak.Unity.MCP.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.Unity.MCP.Server
{
    public class McpServerService : IHostedService
    {
        readonly ILogger<McpServerService> _logger;
        readonly IMcpRunner _mcpRunner;
        readonly IRemoteApp _remoteApp;
        readonly ILocalServer _localServer;

        public IMcpRunner McpRunner => _mcpRunner;
        public IRemoteApp RemoteApp => _remoteApp;
        public ILocalServer LocalServer => _localServer;

        public static McpServerService? Instance { get; private set; }

        public McpServerService(ILogger<McpServerService> logger, IMcpRunner mcpRunner, IRemoteApp remoteApp, ILocalServer localServer, IEndpointRouteBuilder endpoints)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogTrace("Ctor.");
            _mcpRunner = mcpRunner ?? throw new ArgumentNullException(nameof(mcpRunner));
            _remoteApp = remoteApp ?? throw new ArgumentNullException(nameof(remoteApp));
            _localServer = localServer ?? throw new ArgumentNullException(nameof(localServer));

            const string endpoint = Consts.Hub.DefaultEndpoint; // TODO: add reading from configs (json file and env variables)

            endpoints.MapHub<LocalServer>(endpoint + Consts.Hub.LocalServer, options =>
            {
                options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets | Microsoft.AspNetCore.Http.Connections.HttpTransportType.ServerSentEvents;
                options.ApplicationMaxBufferSize = 1024 * 1024 * 10; // 10 MB
                options.TransportMaxBufferSize = 1024 * 1024 * 10; // 10 MB
            });
            endpoints.MapHub<RemoteApp>(endpoint + Consts.Hub.RemoteApp, options =>
            {
                options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets | Microsoft.AspNetCore.Http.Connections.HttpTransportType.ServerSentEvents;
                options.ApplicationMaxBufferSize = 1024 * 1024 * 10; // 10 MB
                options.TransportMaxBufferSize = 1024 * 1024 * 10; // 10 MB
            });

            if (Instance != null)
                throw new InvalidOperationException($"{typeof(McpServerService).Name} is already initialized.");
            Instance = this;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogTrace("StartAsync.");
            // Force the creation of the IMcpApp instance
            // Any initialization logic can go here if needed
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogTrace("StopAsync.");
            Instance = null;
            McpPlugin.StaticDispose();
            return Task.CompletedTask;
        }
    }
}
#endif