#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.Unity.MCP.Server
{
    public class BaseHub : Hub
    {
        protected static readonly List<string> ConnectedClients = new();

        protected readonly ILogger _logger;

        protected BaseHub(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogTrace("Ctor.");
        }

        public override Task OnConnectedAsync()
        {
            ConnectedClients.Add(Context.ConnectionId);
            _logger.LogInformation($"Client connected: {Context.ConnectionId}");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            ConnectedClients.Remove(Context.ConnectionId);
            _logger.LogInformation($"Client disconnected: {Context.ConnectionId}");
            return base.OnDisconnectedAsync(exception);
        }

        protected ISingleClientProxy? GetActiveClient()
        {
            var connectionId = ConnectedClients.FirstOrDefault();
            if (connectionId == null)
                return null;

            var client = Clients.Client(connectionId);
            if (client == null)
            {
                _logger.LogDebug($"Client {connectionId} is not available. Removing from connected clients.");
                ConnectedClients.Remove(connectionId);
                return GetActiveClient();
            }
            _logger.LogTrace($"Client {connectionId} is available.");
            return client;
        }
    }
}