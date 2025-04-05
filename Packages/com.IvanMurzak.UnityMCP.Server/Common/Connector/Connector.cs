using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace com.IvanMurzak.UnityMCP.Common.API
{
    public partial class Connector : IConnector
    {
        public const string Version = "0.1.0";

        readonly ILogger<Connector> _logger;
        readonly ConnectorConfig _config;
        readonly IConnectorReceiver _receiver;
        readonly IConnectorSender _sender;

        public Status ReceiverStatus => _receiver.GetStatus;
        public Status SenderStatus => _sender.GetStatus;

        public Connector(ILogger<Connector> logger, IConnectorReceiver receiver, IConnectorSender sender, IOptions<ConnectorConfig> configOptions)
        {
            _logger = logger;
            _logger.LogTrace($"Ctor. Version: {Version}");

            _receiver = receiver;
            _sender = sender;

            _config = configOptions.Value;
            _logger.LogTrace($"Options. {_config}");

            if (HasInstance)
            {
                _logger.LogError("Connector already created. Use Singleton instance.");
                return;
            }

            instance = this;
        }

        public void Connect()
        {
            _logger.LogTrace("Connect");
            _receiver.Connect();
            _sender.Connect();
        }

        public Task<string?> Send(string message, CancellationToken cancellationToken = default)
        {
            var receiverTask = _receiver.Receive(cancellationToken);
            _sender.Send(message, cancellationToken);
            return receiverTask;
        }

        public void Disconnect()
        {
            _logger.LogTrace("Disconnect");
            Dispose();
        }
        public void Dispose()
        {
            _logger.LogTrace("Dispose");
        }
        ~Connector() => Dispose();
    }
}