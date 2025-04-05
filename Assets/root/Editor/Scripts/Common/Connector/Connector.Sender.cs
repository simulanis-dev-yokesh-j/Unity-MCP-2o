using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace com.IvanMurzak.UnityMCP.Common.API
{
    public partial class Connector : IConnector
    {
        public class Sender : IConnectorSender
        {
            TcpClient? tcpClient;
            NetworkStream? stream;

            readonly ILogger<Sender> _logger;
            readonly ConnectorConfig _config;

            public Status GetStatus { get; protected set; } = Status.Disconnected;

            public Sender(ILogger<Sender> logger, IOptions<ConnectorConfig> configOptions)
            {
                _logger = logger;
                _config = configOptions.Value;
                _logger.LogTrace("Ctor. {0}", _config);
            }

            public void Disconnect()
            {
                _logger.LogTrace("Disconnect");
                Clear();
            }

            public async Task<string?> Send(string data, CancellationToken cancellationToken = default)
            {
                try
                {
                    BuildConnectionIfNeeded(cancellationToken).Wait(cancellationToken);

                    if (stream == null || !stream.CanWrite || !stream.CanRead)
                    {
                        _logger.LogError("Stream is not available for writing or reading.");
                        return null;
                    }

                    await TcpUtils.SendAsync(stream, data, cancellationToken);
                    return await TcpUtils.ReadResponseAsync(stream, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogTrace("SendData operation canceled.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "SendData failed: {0}", ex.Message);
                }
                return null;
            }
            Task BuildConnectionIfNeeded(CancellationToken cancellationToken)
            {
                if (tcpClient == null || !tcpClient.Connected || stream == null)
                {
                    _logger.LogInformation("Connection is not established.");
                    return BuildConnection(cancellationToken);
                }
                return Task.CompletedTask;
            }
            async Task BuildConnection(CancellationToken cancellationToken)
            {
                Clear();
                var port = _config.ConnectionType == ConnectionRole.Unity
                    ? _config.PortUnity
                    : _config.PortServer;
                _logger.LogTrace("Attempting to connect...: {0}:{1}", _config.IPAddress, port);

                tcpClient = new TcpClient();
                await tcpClient.ConnectAsync(_config.IPAddress, port);
                stream = tcpClient.GetStream();
                GetStatus = Status.Connected;
                _logger.LogInformation("Connected to server(receiver): {0}:{1}", _config.IPAddress, port);
            }
            void Clear()
            {
                _logger.LogTrace("Clear");
                tcpClient?.Close();
                tcpClient?.Dispose();
                tcpClient = null;
                stream?.Close();
                stream = null;

                GetStatus = Status.Disconnected;
            }

            public void Dispose()
            {
                _logger.LogTrace("Dispose");
                Clear();
            }

            ~Sender() => Dispose();
        }
    }
}