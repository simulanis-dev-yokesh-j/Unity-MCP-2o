using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using R3;

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
            readonly Subject<string?> _onReceivedData = new();

            public Status GetStatus { get; protected set; } = Status.Disconnected;
            public Observable<string?> OnReceivedData => _onReceivedData;

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
                GetStatus = Status.Disconnected;
            }

            async Task ReceiveData(CancellationToken cancellationToken)
            {

                var port = _config.ConnectionType == ConnectionRole.Unity
                    ? _config.PortServer
                    : _config.PortUnity;

                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        BuildConnectionIfNeeded(port);

                        if (tcpListener == null)
                        {
                            _logger.LogWarning("TcpListener is null. Exiting.");
                            continue;
                        }
                        _logger.LogTrace("Waiting for incoming(sender) connections... {0}:{1}.", _config.IPAddress, port);
                        var client = await tcpListener.AcceptTcpClientAsync();
                        _logger.LogInformation("Client(sender) connected, {0}:{1}.", _config.IPAddress, port);

                        try
                        {
                            using (var stream = client.GetStream())
                            {
                                var receivedData = await TcpUtils.ReadResponseAsync(stream, cancellationToken);
                                _logger.LogTrace("TcpListener Received data: {0}", receivedData);
                                _onReceivedData.OnNext(receivedData);
                                await TcpUtils.SendAsync(stream, Consts.Command.ResponseCode.Success, cancellationToken);
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
                        Disconnect();
                    }
                    catch (OperationCanceledException)
                    {
                        _logger.LogTrace("TcpListener operation canceled.");
                        Disconnect();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "TcpListener failed: {0}", ex.Message);
                        Disconnect();
                    }

                    await Task.Delay(5000, cancellationToken); // Retry every 5 seconds
                }
            }

            void BuildConnectionIfNeeded(int port)
            {
                if (tcpListener == null)
                {
                    _logger.LogTrace("Initializing TcpListener...");
                    tcpListener = new TcpListener(_config.IPAddress, port);
                }

                if (tcpListener.Server != null && !tcpListener.Server.IsBound && GetStatus != Status.Connecting)
                {
                    _logger.LogTrace("Starting TcpListener...");
                    tcpListener.Start();
                    GetStatus = Status.Connecting;
                    _logger.LogInformation("TcpListener started on  {0}:{1}.", _config.IPAddress, port);
                }
            }

            public void Dispose()
            {
                _onReceivedData.Dispose();
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