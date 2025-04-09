#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using com.IvanMurzak.Unity.MCP.Common.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using R3;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public partial class Connector : IConnector
    {
        public class Receiver : IConnectorReceiver
        {
            Task? taskConnection;
            TcpListener? tcpListener;
            CancellationTokenSource? cancellationTokenSource;

            readonly ILogger<Receiver> _logger;
            readonly IToolDispatcher _commandDispatcher;
            readonly IResourceDispatcher _resourceDispatcher;
            readonly ConnectorConfig _config;
            readonly Subject<IRequestData?> _onReceivedData = new();

            public Status GetStatus { get; protected set; } = Status.Disconnected;
            public Observable<IRequestData?> OnReceivedData => _onReceivedData;

            public Receiver(ILogger<Receiver> logger, IToolDispatcher commandDispatcher, IResourceDispatcher resourceDispatcher, IOptions<ConnectorConfig> configOptions)
            {
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
                _logger.LogTrace("Ctor. {0}", configOptions.Value);

                _config = configOptions.Value ?? throw new ArgumentNullException(nameof(configOptions));
                _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
                _resourceDispatcher = resourceDispatcher ?? throw new ArgumentNullException(nameof(resourceDispatcher));
            }

            public void Connect()
            {
                _logger.LogTrace("Connecting...");
                cancellationTokenSource?.Cancel();
                cancellationTokenSource = new CancellationTokenSource();
                taskConnection = Task.Run(() => ListenInLoop(cancellationTokenSource.Token));
            }

            public void Disconnect()
            {
                _logger.LogTrace("Disconnecting...");
                cancellationTokenSource?.Cancel();
                tcpListener?.Stop();
                tcpListener = null;
                GetStatus = Status.Disconnected;
                _logger.LogInformation("Disconnected");
            }

            async Task ListenInLoop(CancellationToken cancellationToken)
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
                            _logger.LogWarning("Connection skip. TcpListener is null.");
                            continue;
                        }
                        _logger.LogDebug("Waiting for incoming connections {0}:{1}.", _config.IPAddress, port);
                        var client = await tcpListener.AcceptTcpClientAsync();
                        _logger.LogDebug("Connected, {0}:{1}.", _config.IPAddress, port);

                        try
                        {
                            using (var stream = client.GetStream())
                            {
                                var receivedData = await TcpUtils.ReadResponseAsync(stream, cancellationToken);
                                _logger.LogTrace("Received data: {0}", receivedData);

                                var requestData = receivedData.ParseRequestData();
                                if (requestData == null)
                                {
                                    _logger.LogWarning("Received data is null. Ignoring.");
                                    continue;
                                }

                                _onReceivedData.OnNext(requestData);

                                if (requestData?.Command != null)
                                {
                                    var result = _commandDispatcher.Dispatch(requestData.Command);
                                    await TcpUtils.SendAsync(stream, result.ToJson(), cancellationToken);
                                }
                                else if (requestData?.ResourceContents != null)
                                {
                                    var result = _resourceDispatcher.Dispatch(requestData.ResourceContents);
                                    _logger.LogTrace("Resource contents: {0}", result.Length);
                                    var response = ResponseData.CreateResourceContents(message: "[Success]", result);
                                    await TcpUtils.SendAsync(stream, response.ToJson(), cancellationToken);
                                }
                                else if (requestData?.ListResources != null)
                                {
                                    var result = _resourceDispatcher.Dispatch(requestData.ListResources);
                                    _logger.LogDebug("List resources: {0}", result.Length);
                                    foreach (var item in result)
                                        _logger.LogDebug("List resource: {0}", item.uri);
                                    var response = ResponseData.CreateListResources(message: "[Success]", result);
                                    await TcpUtils.SendAsync(stream, response.ToJson(), cancellationToken);
                                }
                                else if (requestData?.ListResourceTemplates != null)
                                {
                                    var result = _resourceDispatcher.Dispatch(requestData.ListResourceTemplates);
                                    _logger.LogTrace("List resource templates: {0}", result.Length);
                                    var response = ResponseData.CreateListResourceTemplates(message: "[Success]", result);
                                    await TcpUtils.SendAsync(stream, response.ToJson(), cancellationToken);
                                }
                                else if (requestData?.Notification != null)
                                {
                                    // var result = _notificationDispatcher.Dispatch(dataPackage.Response);
                                    // await TcpUtils.SendAsync(stream, result.ToJson(), cancellationToken);
                                    await TcpUtils.SendAsync(stream, Consts.Command.ResponseCode.Error, cancellationToken);
                                }
                                else
                                {
                                    _logger.LogWarning("Received data is not a command or notification. Ignoring.");
                                    await TcpUtils.SendAsync(stream, Consts.Command.ResponseCode.Error, cancellationToken);
                                }
                            }
                        }
                        finally
                        {
                            _logger.LogTrace("Data receiving and processing is completed.");
                            client.Close();
                        }
                    }
                    catch (ObjectDisposedException ex)
                    {
                        _logger.LogTrace(ex, "TcpListener disposed. Ignoring.");
                    }
                    catch (OperationCanceledException)
                    {
                        _logger.LogTrace("Stop listening. Canceled.");
                        Disconnect();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Stop listening. Error: {0}", ex.Message);
                        Disconnect();
                    }

                    await Task.Yield();  // (5000, cancellationToken); // Retry every 5 seconds
                }
            }

            void BuildConnectionIfNeeded(int port)
            {
                if (tcpListener == null)
                {
                    _logger.LogTrace("Initializing TcpListener...");
                    tcpListener = new TcpListener(_config.IPAddress, port);

                    // Enable socket reuse to avoid "address already in use" errors
                    tcpListener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
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
                tcpListener?.Server?.Close();
                tcpListener = null;
                GetStatus = Status.Disconnected;
            }

            ~Receiver() => Dispose();
        }
    }
}