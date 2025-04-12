using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public class HubConnectionLogger : IDisposable
    {
        private readonly ILogger _logger;
        private readonly HubConnection _hubConnection;

        public HubConnectionLogger(ILogger logger, HubConnection hubConnection)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _hubConnection = hubConnection ?? throw new ArgumentNullException(nameof(hubConnection));

            if (_logger.IsEnabled(LogLevel.Trace))
                SubscribeToHubConnectionEvents();
        }

        void SubscribeToHubConnectionEvents()
        {
            _hubConnection.Closed += OnClosedConnection;
            _hubConnection.Reconnecting += OnReconnecting;
            _hubConnection.Reconnected += OnReconnected;
        }

        Task OnClosedConnection(Exception? ex)
        {
            _logger.LogTrace("HubConnection closed. Exception: {0}", ex?.Message);
            if (ex != null)
                _logger.LogError("Error in Closed event subscription: {0}", ex.Message);
            return Task.CompletedTask;
        }
        Task OnReconnecting(Exception? ex)
        {
            _logger.LogTrace("HubConnection reconnecting.");
            if (ex != null)
                _logger.LogError("Error during reconnecting: {0}", ex.Message);
            return Task.CompletedTask;
        }
        Task OnReconnected(string? connectionId)
        {
            _logger.LogTrace("HubConnection reconnected with id {0}.", connectionId);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _hubConnection.Closed -= OnClosedConnection;
            _hubConnection.Reconnecting -= OnReconnecting;
            _hubConnection.Reconnected -= OnReconnected;
        }
    }
}