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
            _logger.LogTrace("Ctor. Version: {0}", Version);

            _receiver = receiver;
            _sender = sender;

            _config = configOptions.Value;
            _logger.LogTrace("Options. {0}", _config);

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