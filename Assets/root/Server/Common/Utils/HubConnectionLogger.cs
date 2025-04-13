#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using R3;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public class HubConnectionLogger : HubConnectionObservable, IDisposable
    {
        readonly ILogger _logger;
        readonly CompositeDisposable _disposables = new();

        public HubConnectionLogger(ILogger logger, HubConnection hubConnection) : base(hubConnection)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            Closed
                .Where(x => _logger.IsEnabled(LogLevel.Debug))
                .Subscribe(ex =>
                {
                    _logger.LogTrace("HubConnection OnClosed. Exception: {0}", ex?.Message);
                    if (ex != null)
                        _logger.LogError("Error in Closed event subscription: {0}", ex.Message);
                })
                .AddTo(_disposables);

            Reconnecting
                .Where(x => _logger.IsEnabled(LogLevel.Debug))
                .Subscribe(ex =>
                {
                    _logger.LogTrace("HubConnection OnReconnecting.");
                    if (ex != null)
                        _logger.LogError("Error during reconnecting: {0}", ex.Message);
                })
                .AddTo(_disposables);

            Reconnected
                .Where(x => _logger.IsEnabled(LogLevel.Debug))
                .Subscribe(connectionId =>
                {
                    _logger.LogTrace("HubConnection OnReconnected with id {0}.", connectionId);
                })
                .AddTo(_disposables);
        }

        public override void Dispose()
        {
            base.Dispose();
            _disposables.Dispose();
        }
    }
}