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
        public const string Version = "0.1.0";

        Task? taskConnection;
        Task? taskDataSend;
        Task? taskDataReceive;
        TcpClient? tcpClient;
        TcpListener? tcpListener;
        NetworkStream? networkStream;
        CancellationTokenSource? cancellationTokenSource;

        Queue<string> sendQueue = new();

        readonly ILogger<Connector> _logger;
        readonly ConnectorConfig _config;

        public Status GetStatus { get; protected set; } = Status.Disconnected;

        public Connector(ILogger<Connector> logger, IOptions<ConnectorConfig> configOptions)
        {
            _logger = logger;
            _logger.LogTrace($"Ctor. Version: {Version}");

            _config = configOptions.Value;
            _logger.LogTrace($"Options. {_config}");

            if (HasInstance)
            {
                _logger.LogError("Connector already created. Use Singleton instance.");
                return;
            }

            instance = this;
        }

        public void Connect()
        {
            _logger.LogTrace("Connect");
            cancellationTokenSource?.Cancel();
            cancellationTokenSource = new CancellationTokenSource();
            taskConnection = Task.Run(() => MonitorConnection(cancellationTokenSource.Token));
            taskDataReceive = Task.Run(() => ReceiveData(cancellationTokenSource.Token));
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
                        await tcpClient.ConnectAsync(_config.IPAddress, _config.Port);
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
        async Task SendData(string data, CancellationToken cancellationToken)
        {
            try
            {
                if (tcpClient != null && tcpClient.Connected && networkStream != null)
                {
                    var buffer = System.Text.Encoding.UTF8.GetBytes(data);
                    await networkStream.WriteAsync(buffer, 0, buffer.Length, cancellationToken);
                    _logger.LogInformation($"Sent data: {data}");
                }
                else
                {
                    _logger.LogWarning("TcpClient is not connected or NetworkStream is null.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"SendData failed: {ex.Message}");
            }
        }
        async Task ReceiveData(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        if (tcpListener == null)
                        {
                            _logger.LogTrace("Initializing TcpListener...");
                            tcpListener = new TcpListener(_config.IPAddress, _config.Port);
                            tcpListener.Start();
                            _logger.LogInformation($"TcpListener started on {_config.IPAddress}:{_config.Port}");
                        }

                        _logger.LogTrace("Waiting for incoming connections...");
                        var client = await tcpListener.AcceptTcpClientAsync();
                        _logger.LogInformation("Client connected.");

                        try
                        {
                            using (var stream = client.GetStream())
                            {
                                var buffer = new byte[1024];
                                int bytesRead;

                                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
                                {
                                    var receivedData = System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead);
                                    _logger.LogInformation($"Received data: {receivedData}");
                                    // Process the received data here
                                }
                            }
                        }
                        finally
                        {
                            _logger.LogInformation("Client disconnected.");
                            client.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"TcpListener failed: {ex.Message}");
                        GetStatus = Status.Disconnected;
                    }

                    await Task.Delay(5000, cancellationToken); // Retry every 5 seconds
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ReceiveData failed: {ex.Message}");
            }
            finally
            {
                tcpListener?.Stop();
            }
        }
        public void Disconnect()
        {
            _logger.LogTrace("Disconnect");
            cancellationTokenSource?.Cancel();
            tcpClient?.Close();
            tcpListener?.Stop();
            Dispose();
        }
        public void Dispose()
        {
            _logger.LogTrace("Dispose");
            cancellationTokenSource?.Dispose();
            cancellationTokenSource = null;
            tcpClient?.Close();
            tcpClient?.Dispose();
            tcpListener?.Stop();
            tcpListener = null;
            networkStream?.Close();
            networkStream = null;
            tcpClient = null;
            GetStatus = Status.Disconnected;
        }
        ~Connector() => Dispose();
    }
}