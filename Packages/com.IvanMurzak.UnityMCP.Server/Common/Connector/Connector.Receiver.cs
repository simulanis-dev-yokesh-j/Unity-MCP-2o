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
        public class Receiver : IConnectorReceiver
        {
            Task? taskConnection;
            TcpListener? tcpListener;
            CancellationTokenSource? cancellationTokenSource;

            readonly ILogger<Receiver> _logger;
            readonly ConnectorConfig _config;

            public Status GetStatus { get; protected set; } = Status.Disconnected;

            public Receiver(ILogger<Receiver> logger, IOptions<ConnectorConfig> configOptions)
            {
                _logger = logger;
                _config = configOptions.Value;
                _logger.LogTrace("Ctor. {0}", _config);
            }

            public void Connect()
            {
                _logger.LogTrace("Connect");
                cancellationTokenSource?.Cancel();
                cancellationTokenSource = new CancellationTokenSource();
                taskConnection = Task.Run(() => ReceiveData(cancellationTokenSource.Token));
            }

            public void Disconnect()
            {
                _logger.LogTrace("Disconnect");
                cancellationTokenSource?.Cancel();
                tcpListener?.Stop();
                tcpListener = null;
                Dispose();
            }

            async Task ReceiveData(CancellationToken cancellationToken)
            {
                try
                {
                    var port = _config.ConnectionType == ConnectionRole.Unity
                        ? _config.PortServer
                        : _config.PortUnity;

                    if (tcpListener == null)
                    {
                        _logger.LogTrace("Initializing TcpListener...");
                        tcpListener = new TcpListener(_config.IPAddress, port);
                    }

                    if (tcpListener.Server != null && !tcpListener.Server.IsBound)
                    {
                        _logger.LogTrace("Starting TcpListener...");
                        tcpListener.Start();
                        _logger.LogInformation("TcpListener started on  {0}:{1}.", _config.IPAddress, port);
                    }
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        try
                        {
                            _logger.LogTrace("Waiting for incoming(sender) connections... {0}:{1}.", _config.IPAddress, port);
                            if (tcpListener == null)
                            {
                                _logger.LogWarning("TcpListener is null. Exiting.");
                                break;
                            }
                            var client = await tcpListener.AcceptTcpClientAsync();
                            _logger.LogInformation("Client(sender) connected, {0}:{1}.", _config.IPAddress, port);

                            try
                            {
                                using (var stream = client.GetStream())
                                {
                                    var receivedData = await TcpUtils.ReadResponseAsync(stream, cancellationToken);
                                    _logger.LogTrace("TcpListener Received full data: {0}", receivedData);
                                }
                            }
                            finally
                            {
                                _logger.LogInformation("Client(sender) disconnected.");
                                client.Close();
                            }
                        }
                        catch (ObjectDisposedException ex)
                        {
                            _logger.LogTrace(ex, "TcpListener disposed object exception. Ignoring.");
                            GetStatus = Status.Disconnected;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "TcpListener failed: {0}", ex.Message);
                            GetStatus = Status.Disconnected;
                        }

                        await Task.Delay(5000, cancellationToken); // Retry every 5 seconds
                    }
                }
                catch (OperationCanceledException)
                {
                    _logger.LogTrace("TcpListener operation canceled.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "TcpListener failed {0}", ex.Message);
                }
                finally
                {
                    tcpListener?.Stop();
                    tcpListener = null;
                }
            }

            public void Dispose()
            {
                cancellationTokenSource?.Cancel();
                cancellationTokenSource?.Dispose();
                cancellationTokenSource = null;
                tcpListener?.Stop();
                tcpListener = null;
                GetStatus = Status.Disconnected;
            }

            ~Receiver() => Dispose();
        }
    }
}