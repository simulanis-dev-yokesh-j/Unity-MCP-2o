using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using System.Net;
using Microsoft.Extensions.Options;

namespace com.IvanMurzak.UnityMCP.Common.API
{
    public partial class Connector : IConnector
    {
        public const string Version = "0.1.0";

        TcpClient tcpClient;
        NetworkStream networkStream;

        readonly ILogger _logger;

        public Status GetStatus { get; protected set; } = Status.Disconnected;

        internal Connector(ILogger logger, IOptions<ConnectorConfig> configOptions)
        {
            _logger = logger;
            _logger.LogTrace($"Ctor. {Version}");

            var config = configOptions.Value;

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