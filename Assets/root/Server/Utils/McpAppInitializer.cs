#if !IGNORE
using System;
using System.Threading;
using System.Threading.Tasks;
using com.IvanMurzak.Unity.MCP.Common;
using Microsoft.Extensions.Hosting;

namespace com.IvanMurzak.Unity.MCP.Server
{
    public class McpAppInitializer : IHostedService
    {
        readonly IMcpPlugin _mcpApp;
        readonly ILocalServer _localServer;

        public ILocalServer LocalServer => _localServer;

        public static McpAppInitializer? Instance { get; private set; }

        public McpAppInitializer(IMcpPlugin mcpApp, ILocalServer localServer)
        {
            _mcpApp = mcpApp ?? throw new ArgumentNullException(nameof(mcpApp));
            _localServer = localServer ?? throw new ArgumentNullException(nameof(localServer));

            if (Instance != null)
                throw new InvalidOperationException("McpAppInitializer is already initialized.");
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