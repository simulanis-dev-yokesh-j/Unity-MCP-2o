using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using R3;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public class HubConnectionLogger : IDisposable
    {
        private readonly ILogger _logger;
        private readonly HubConnection _hubConnection;
        private readonly CompositeDisposable _disposables = new();

        public HubConnectionLogger(ILogger logger, HubConnection hubConnection)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _hubConnection = hubConnection ?? throw new ArgumentNullException(nameof(hubConnection));

            if (_logger.IsEnabled(LogLevel.Trace))
                SubscribeToHubConnectionEvents();
        }

        private void SubscribeToHubConnectionEvents()
        {
            // Subscribe to the Closed event
            Observable.FromEvent<Func<Exception?, Task>, Exception?>(
                    handler => ex => { handler(ex); return Task.CompletedTask; },
                    handler => _hubConnection.Closed += handler,
                    handler => _hubConnection.Closed -= handler)
                .Subscribe(ex =>
                {
                    _logger.LogTrace("HubConnection closed. Exception: {0}", ex?.Message);
                    if (ex != null)
                        _logger.LogError("Error in Closed event subscription: {0}", ex.Message);
                })
                .AddTo(_disposables);

            // Subscribe to the Reconnecting event
            Observable.FromEvent<Func<Exception?, Task>, Exception?>(
                    handler => ex => { handler(ex); return Task.CompletedTask; },
                    handler => _hubConnection.Reconnecting += handler,
                    handler => _hubConnection.Reconnecting -= handler)
                .Subscribe(ex =>
                {
                    _logger.LogTrace("HubConnection reconnecting. Exception: {0}", ex?.Message);
                    if (ex != null)
                        _logger.LogError("Error in Reconnecting event subscription: {0}", ex.Message);
                })
                .AddTo(_disposables);

            // Subscribe to the Reconnected event
            Observable.FromEvent<Func<string?, Task>, string?>(
                    handler => connectionId => { handler(connectionId); return Task.CompletedTask; },
                    handler => _hubConnection.Reconnected += handler,
                    handler => _hubConnection.Reconnected -= handler)
                .Subscribe(connectionId =>
                {
                    _logger.LogTrace("HubConnection reconnected. ConnectionId: {0}", connectionId);
                })
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}