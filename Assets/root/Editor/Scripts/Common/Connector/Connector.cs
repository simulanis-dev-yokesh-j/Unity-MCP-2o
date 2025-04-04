using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace com.IvanMurzak.UnityMCP.Common.API
{
    public partial class Connector : IConnector
    {
        public const string Version = "0.1.0";

        TcpClient tcpClient;
        NetworkStream networkStream;

        readonly ILogger<Connector> _logger;

        public Status GetStatus { get; protected set; } = Status.Disconnected;

        public Connector(ILogger<Connector> logger, IOptions<ConnectorConfig> configOptions)
        {
            _logger = logger;
            _logger.LogTrace($"Ctor. Version: {Version}");

            var config = configOptions.Value;

            _logger.LogTrace($"Options. {config}");
            tcpClient = new TcpClient(config.Hostname, config.Port);
            networkStream = tcpClient.GetStream();
        }

        public void Connect()
        {

        }
        public void Disconnect()
        {

        }
        public void Dispose()
        {

        }
        ~Connector() => Dispose();
    }
}