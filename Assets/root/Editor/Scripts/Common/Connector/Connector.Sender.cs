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
        public class Sender : IConnectorSender
        {
            // Task? taskConnection;
            Task? taskDataSend;
            TcpClient? tcpClient;
            NetworkStream? networkStream;
            // CancellationTokenSource? cancellationTokenSource;
            Queue<string> sendQueue = new();

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
                Dispose();
            }

            async Task MonitorConnection(CancellationToken cancellationToken)
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        if (tcpClient == null || !tcpClient.Connected)
                        {

                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Connection failed: {ex.Message}");
                        GetStatus = Status.Disconnected;
                    }

                    await Task.Delay(5000, cancellationToken); // Retry every 5 seconds
                }
            }
            public Task Send(string data, CancellationToken cancellationToken = default)
            {
                _logger.LogTrace($"Send. Data: {data}");
                lock (sendQueue)
                {
                    sendQueue.Enqueue(data);
                    if (taskDataSend == null || taskDataSend.IsCompleted)
                    {
                        taskDataSend = Task.Run(async () =>
                        {
                            while (sendQueue.Count > 0 && !cancellationToken.IsCancellationRequested)
                            {
                                string dataToSend;
                                lock (sendQueue)
                                {
                                    dataToSend = sendQueue.Dequeue();
                                }
                                await SendData(dataToSend, cancellationToken);
                            }
                        }, cancellationToken);
                    }
                }
                return taskDataSend;
            }
            async Task<bool> SendData(string data, CancellationToken cancellationToken)
            {
                try
                {
                    await BuildConnectionIfNeeded(cancellationToken);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(data);
                    await networkStream.WriteAsync(buffer, 0, buffer.Length, cancellationToken);
                    _logger.LogInformation($"Sent data: {data}");
                }
                catch (OperationCanceledException)
                {
                    _logger.LogTrace("SendData operation canceled.");
                    return false;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"SendData failed: {ex.Message}");
                    return false;
                }
                return true;
            }
            Task BuildConnectionIfNeeded(CancellationToken cancellationToken)
            {
                if (tcpClient == null || !tcpClient.Connected || networkStream == null)
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
                networkStream = tcpClient.GetStream();
                GetStatus = Status.Connected;
                _logger.LogInformation("Connected to server(receiver): {0}:{1}", _config.IPAddress, port);
            }
            void Clear()
            {
                _logger.LogTrace("Clear");
                tcpClient?.Close();
                tcpClient?.Dispose();
                tcpClient = null;
                networkStream?.Close();
                networkStream = null;

                // cancellationTokenSource?.Cancel();
                // cancellationTokenSource?.Dispose();
                // cancellationTokenSource = null;

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