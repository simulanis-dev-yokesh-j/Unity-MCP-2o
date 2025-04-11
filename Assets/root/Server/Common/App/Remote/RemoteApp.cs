#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using com.IvanMurzak.Unity.MCP.Common.Data;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public class RemoteApp : IRemoteApp
    {
        protected readonly ILogger<RemoteApp> _logger;
        protected readonly Func<Task<HubConnection>> _getHubConnection;

        public RemoteApp(ILogger<RemoteApp> logger, Func<Task<HubConnection>> getHubConnection)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _getHubConnection = getHubConnection ?? throw new ArgumentNullException(nameof(getHubConnection));
        }

        public async Task<IResponseData<IResponseCallTool>> RunCallTool(IRequestCallTool data, CancellationToken cancellationToken = default)
        {
            if (data == null)
                return ResponseData<IResponseCallTool>.Error("Tool data is null.")
                    .Log(_logger);

            if (string.IsNullOrEmpty(data.Name))
                return ResponseData<IResponseCallTool>.Error("Tool.Name is null.")
                    .Log(_logger);
            try
            {
                var hubConnection = await _getHubConnection();
                if (hubConnection == null)
                    return ResponseData<IResponseCallTool>.Error("Can't establish connection with Remote.")
                        .Log(_logger);

                if (_logger.IsEnabled(LogLevel.Information))
                {
                    var message = data.Arguments == null
                        ? $"Run tool '{data.Name}' with no parameters."
                        : $"Run tool '{data.Name}' with parameters[{data.Arguments.Count}]:\n{string.Join(",\n", data.Arguments)}";
                    _logger.LogInformation(message);
                }
                var result = await hubConnection.InvokeAsync<ResponseCallTool>(Consts.RCP.RunCallTool, data, cancellationToken);
                if (result == null)
                    return ResponseData<IResponseCallTool>.Error($"Tool '{data.Name}' returned null result.")
                        .Log(_logger);

                return result.Log(_logger).Pack();
            }
            catch (Exception ex)
            {
                return ResponseData<IResponseCallTool>.Error($"Failed to run tool '{data.Name}'. Exception: {ex}")
                    .Log(_logger, ex);
            }
        }

        public async Task<IResponseData<List<IResponseListTool>>> RunListTool(IRequestListTool data, CancellationToken cancellationToken = default)
        {
            try
            {
                var hubConnection = await _getHubConnection();
                if (hubConnection == null)
                    return ResponseData<List<IResponseListTool>>.Error("Can't establish connection with Remote.")
                        .Log(_logger);

                var result = await hubConnection.InvokeAsync<List<ResponseListTool>>(Consts.RCP.RunListTool, data, cancellationToken);
                if (result == null)
                    return ResponseData<List<IResponseListTool>>.Error($"'{Consts.RCP.RunListTool}' returned null result.")
                        .Log(_logger);

                return result
                    .Cast<IResponseListTool>()
                    .ToList()
                    .Log(_logger)
                    .Pack();
            }
            catch (Exception ex)
            {
                return ResponseData<List<IResponseListTool>>.Error($"Failed to run '{Consts.RCP.RunListTool}'. Exception: {ex}")
                    .Log(_logger, ex);
            }
        }

        public async Task<IResponseData<List<IResponseResourceContent>>> RunResourceContent(IRequestResourceContent data, CancellationToken cancellationToken = default)
        {
            if (data == null)
                return ResponseData<List<IResponseResourceContent>>.Error("Resource content data is null.")
                    .Log(_logger);

            if (string.IsNullOrEmpty(data.Uri))
                return ResponseData<List<IResponseResourceContent>>.Error("Resource content Uri is null.")
                    .Log(_logger);

            try
            {
                var hubConnection = await _getHubConnection();
                if (hubConnection == null)
                    return ResponseData<List<IResponseResourceContent>>.Error("Can't establish connection with Remote.")
                        .Log(_logger);

                var result = await hubConnection.InvokeAsync<List<ResponseListTool>>(Consts.RCP.RunResourceContent, data, cancellationToken);
                if (result == null)
                    return ResponseData<List<IResponseResourceContent>>.Error($"Resource uri: '{data.Uri}' returned null result.")
                        .Log(_logger);

                return result
                    .Cast<IResponseResourceContent>()
                    .ToList()
                    .Log(_logger)
                    .Pack();
            }
            catch (Exception ex)
            {
                return ResponseData<List<IResponseResourceContent>>.Error($"Failed to get resource uri: '{data.Uri}'. Exception: {ex}")
                    .Log(_logger, ex);
            }
        }

        public async Task<IResponseData<List<IResponseListResource>>> RunListResources(IRequestListResources data, CancellationToken cancellationToken = default)
        {
            try
            {
                var hubConnection = await _getHubConnection();
                if (hubConnection == null)
                    return ResponseData<List<IResponseListResource>>.Error("Can't establish connection with Remote.")
                        .Log(_logger);

                var result = await hubConnection.InvokeAsync<List<ResponseListResource>>(Consts.RCP.RunListResources, data, cancellationToken);
                if (result == null)
                    return ResponseData<List<IResponseListResource>>.Error($"'{Consts.RCP.RunListResources}' returned null result.")
                        .Log(_logger);

                return result
                    .Cast<IResponseListResource>()
                    .ToList()
                    .Log(_logger)
                    .Pack();
            }
            catch (Exception ex)
            {
                return ResponseData<List<IResponseListResource>>.Error($"Failed to run '{Consts.RCP.RunListResources}'. Exception: {ex}")
                    .Log(_logger, ex);
            }
        }

        public async Task<IResponseData<List<IResponseResourceTemplate>>> RunResourceTemplates(IRequestListResourceTemplates data, CancellationToken cancellationToken = default)
        {
            try
            {
                var hubConnection = await _getHubConnection();
                if (hubConnection == null)
                    return ResponseData<List<IResponseResourceTemplate>>.Error("Can't establish connection with Remote.")
                        .Log(_logger);

                var result = await hubConnection.InvokeAsync<List<ResponseResourceTemplate>>(Consts.RCP.RunListResourceTemplates, data, cancellationToken);
                if (result == null)
                    return ResponseData<List<IResponseResourceTemplate>>.Error($"'{Consts.RCP.RunListResourceTemplates}' returned null result.")
                        .Log(_logger);

                return result
                    .Cast<IResponseResourceTemplate>()
                    .ToList()
                    .Log(_logger)
                    .Pack();
            }
            catch (Exception ex)
            {
                return ResponseData<List<IResponseResourceTemplate>>.Error($"Failed to run '{Consts.RCP.RunListResourceTemplates}'. Exception: {ex}")
                    .Log(_logger, ex);
            }
        }

        public void Dispose()
        {

        }
    }
}