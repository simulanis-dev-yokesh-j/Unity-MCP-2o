using System;
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
            if (HasInstance)
            {
                _logger.LogError("Connector already created. Use Singleton instance.");
                return;
            }

            instance = this;

            var config = configOptions.Value;

            _logger.LogTrace($"Options. {config}");
            // tcpClient = new TcpClient(config.Hostname, config.Port);
            // networkStream = tcpClient.GetStream();
        }

        public void Connect()
        {
            _logger.LogTrace("Connect");
        }
        public void Disconnect()
        {
            _logger.LogTrace("Disconnect");
        }
        public void Dispose()
        {
            _logger.LogTrace("Dispose");
            tcpClient?.Close();
            tcpClient?.Dispose();
            networkStream?.Close();
        }
        ~Connector() => Dispose();
    }
}