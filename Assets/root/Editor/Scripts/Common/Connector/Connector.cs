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
        public const string Version = "0.1.0";

        Task connectionTask;
        TcpClient tcpClient;
        NetworkStream networkStream;
        CancellationTokenSource cancellationTokenSource;

        readonly ILogger<Connector> _logger;
        readonly ConnectorConfig _config;

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

            _config = configOptions.Value;
            _logger.LogTrace($"Options. {_config}");
            // tcpClient = new TcpClient(config.Hostname, config.Port);
            // networkStream = tcpClient.GetStream();
        }

        public void Connect()
        {
            _logger.LogTrace("Connect");
            cancellationTokenSource = new CancellationTokenSource();
            connectionTask = Task.Run(() => MonitorConnection(cancellationTokenSource.Token));
        }
        async Task MonitorConnection(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    if (tcpClient == null || !tcpClient.Connected)
                    {
                        _logger.LogTrace("Attempting to connect...");
                        tcpClient = new TcpClient();
                        await tcpClient.ConnectAsync(_config.Hostname, _config.Port);
                        networkStream = tcpClient.GetStream();
                        GetStatus = Status.Connected;
                        _logger.LogInformation("Connected to server.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Connection failed: {ex.Message}");
                    GetStatus = Status.Disconnected;
                }

                await Task.Delay(5000, cancellationToken); // Retry every 5 seconds
            }
        }
        public void Disconnect()
        {
            _logger.LogTrace("Disconnect");
            cancellationTokenSource?.Cancel();
            Dispose();
        }
        public void Dispose()
        {
            _logger.LogTrace("Dispose");
            tcpClient?.Close();
            tcpClient?.Dispose();
            networkStream?.Close();
            networkStream = null;
            tcpClient = null;
            GetStatus = Status.Disconnected;
        }
        ~Connector() => Dispose();
    }
}