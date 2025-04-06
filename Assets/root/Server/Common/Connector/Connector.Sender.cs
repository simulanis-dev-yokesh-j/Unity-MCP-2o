#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using com.IvanMurzak.Unity.MCP.Common.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace com.IvanMurzak.Unity.MCP.Common
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
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
                _logger.LogTrace("Ctor. {0}", configOptions.Value);

                _config = configOptions.Value ?? throw new ArgumentNullException(nameof(configOptions));
            }

            public void Disconnect()
            {
                _logger.LogTrace("Disconnect");
                Clear();
            }

            public async Task<IResponseData?> Send(IDataPackage data, int retry = 10, CancellationToken cancellationToken = default)
            {
                try
                {
                    BuildConnectionIfNeeded(cancellationToken).Wait(cancellationToken);

                    if (stream == null || !stream.CanWrite || !stream.CanRead)
                    {
                        _logger.LogError("Stream is not available for writing or reading.");
                        return null;
                    }

                    var json = data.ToJson();
                    await TcpUtils.SendAsync(stream, json, cancellationToken);
                    var jsonResponse = await TcpUtils.ReadResponseAsync(stream, cancellationToken);
                    return jsonResponse.ParseResponseData();
                }
                catch (OperationCanceledException)
                {
                    _logger.LogTrace("SendData operation canceled.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "SendData failed: {0}", ex.Message);
                }
                finally
                {
                    if (tcpClient != null && tcpClient.Connected)
                    {
                        _logger.LogTrace("Closing connection.");
                        tcpClient.Close();
                    }
                }
                if (retry > 0)
                    return await Send(data, retry - 1, cancellationToken);
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