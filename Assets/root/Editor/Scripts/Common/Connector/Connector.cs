using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using R3;

namespace com.IvanMurzak.UnityMCP.Common.API
{
    public partial class Connector : IConnector
    {
        public const string Version = "0.1.0";

        readonly ILogger<Connector> _logger;
        readonly IConnectorReceiver _receiver;
        readonly IConnectorSender _sender;

        public Status ReceiverStatus => _receiver.GetStatus;
        public Status SenderStatus => _sender.GetStatus;

        public Observable<string?> OnReceivedData => _receiver.OnReceivedData;

        public Connector(ILogger<Connector> logger, IConnectorReceiver receiver, IConnectorSender sender, IOptions<ConnectorConfig> configOptions)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogTrace("Ctor. Version: {0}", Version);

            _receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));
            _sender = sender ?? throw new ArgumentNullException(nameof(sender));

            if (HasInstance)
            {
                _logger.LogError("Connector already created. Use Singleton instance.");
                return;
            }

            instance = this;
        }

        public void Connect()
        {
            _receiver.Connect();
        }

        public Task<string?> Send(string message, CancellationToken cancellationToken = default)
            => _sender.Send(message, cancellationToken);

        public void Disconnect()
        {
            _receiver.Disconnect();
            _sender.Disconnect();
        }

        public void Dispose()
        {
            _receiver.Dispose();
            _sender.Dispose();
        }
        ~Connector() => Dispose();
    }
}