#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Common.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.Unity.MCP.Server
{
    public class RemoteApp : Hub, IRemoteApp
    {
        protected readonly ILogger<RemoteApp> _logger;
        protected readonly IConnectionManager _connectionManager;

        public RemoteApp(ILogger<RemoteApp> logger, IConnectionManager connectionManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogTrace("Ctor.");
            _connectionManager = connectionManager ?? throw new ArgumentNullException(nameof(connectionManager));
        }

        public async Task<IResponseData<IResponseCallTool>> RunCallTool(IRequestCallTool data, CancellationToken cancellationToken = default)
        {
            if (data == null)
                return ResponseData<IResponseCallTool>.Error(Consts.Guid.Zero, "Tool data is null.")
                    .Log(_logger);

            if (string.IsNullOrEmpty(data.Name))
                return ResponseData<IResponseCallTool>.Error(data.RequestID, "Tool.Name is null.")
                    .Log(_logger);
            try
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    var message = data.Arguments == null
                        ? $"Run tool '{data.Name}' with no parameters."
                        : $"Run tool '{data.Name}' with parameters[{data.Arguments.Count}]:\n{string.Join(",\n", data.Arguments)}";
                    _logger.LogInformation(message);
                }
                var result = await _connectionManager.InvokeAsync<IRequestCallTool, ResponseCallTool>(Consts.RPC.RunCallTool, data, cancellationToken);
                if (result == null)
                    return ResponseData<IResponseCallTool>.Error(data.RequestID, $"Tool '{data.Name}' returned null result.")
                        .Log(_logger);

                return result.Log(_logger).Pack(data.RequestID);
            }
            catch (Exception ex)
            {
                return ResponseData<IResponseCallTool>.Error(data.RequestID, $"Failed to run tool '{data.Name}'. Exception: {ex}")
                    .Log(_logger, ex);
            }
        }

        public async Task<IResponseData<List<IResponseListTool>>> RunListTool(IRequestListTool data, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _connectionManager.InvokeAsync<IRequestListTool, List<ResponseListTool>>(Consts.RPC.RunListTool, data, cancellationToken);
                if (result == null)
                    return ResponseData<List<IResponseListTool>>.Error(data.RequestID, $"'{Consts.RPC.RunListTool}' returned null result.")
                        .Log(_logger);

                return result
                    .Cast<IResponseListTool>()
                    .ToList()
                    .Log(_logger)
                    .Pack(data.RequestID);
            }
            catch (Exception ex)
            {
                return ResponseData<List<IResponseListTool>>.Error(data.RequestID, $"Failed to run '{Consts.RPC.RunListTool}'. Exception: {ex}")
                    .Log(_logger, ex);
            }
        }

        public async Task<IResponseData<List<IResponseResourceContent>>> RunResourceContent(IRequestResourceContent data, CancellationToken cancellationToken = default)
        {
            if (data == null)
                return ResponseData<List<IResponseResourceContent>>.Error(Consts.Guid.Zero, "Resource content data is null.")
                    .Log(_logger);

            if (string.IsNullOrEmpty(data.Uri))
                return ResponseData<List<IResponseResourceContent>>.Error(data.RequestID, "Resource content Uri is null.")
                    .Log(_logger);

            try
            {
                var result = await _connectionManager.InvokeAsync<IRequestResourceContent, List<ResponseResourceContent>>(Consts.RPC.RunResourceContent, data, cancellationToken);
                if (result == null)
                    return ResponseData<List<IResponseResourceContent>>.Error(data.RequestID, $"Resource uri: '{data.Uri}' returned null result.")
                        .Log(_logger);

                return result
                    .Cast<IResponseResourceContent>()
                    .ToList()
                    .Log(_logger)
                    .Pack(data.RequestID);
            }
            catch (Exception ex)
            {
                return ResponseData<List<IResponseResourceContent>>.Error(data.RequestID, $"Failed to get resource uri: '{data.Uri}'. Exception: {ex}")
                    .Log(_logger, ex);
            }
        }

        public async Task<IResponseData<List<IResponseListResource>>> RunListResources(IRequestListResources data, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _connectionManager.InvokeAsync<IRequestListResources, List<ResponseResourceContent>>(Consts.RPC.RunListResources, data, cancellationToken);
                if (result == null)
                    return ResponseData<List<IResponseListResource>>.Error(data.RequestID, $"'{Consts.RPC.RunListResources}' returned null result.")
                        .Log(_logger);

                return result
                    .Cast<IResponseListResource>()
                    .ToList()
                    .Log(_logger)
                    .Pack(data.RequestID);
            }
            catch (Exception ex)
            {
                return ResponseData<List<IResponseListResource>>.Error(data.RequestID, $"Failed to run '{Consts.RPC.RunListResources}'. Exception: {ex}")
                    .Log(_logger, ex);
            }
        }

        public async Task<IResponseData<List<IResponseResourceTemplate>>> RunResourceTemplates(IRequestListResourceTemplates data, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _connectionManager.InvokeAsync<IRequestListResourceTemplates, List<ResponseResourceTemplate>>(Consts.RPC.RunListResourceTemplates, data, cancellationToken);
                if (result == null)
                    return ResponseData<List<IResponseResourceTemplate>>.Error(data.RequestID, $"'{Consts.RPC.RunListResourceTemplates}' returned null result.")
                        .Log(_logger);

                return result
                    .Cast<IResponseResourceTemplate>()
                    .ToList()
                    .Log(_logger)
                    .Pack(data.RequestID);
            }
            catch (Exception ex)
            {
                return ResponseData<List<IResponseResourceTemplate>>.Error(data.RequestID, $"Failed to run '{Consts.RPC.RunListResourceTemplates}'. Exception: {ex}")
                    .Log(_logger, ex);
            }
        }

        public new void Dispose()
        {
            base.Dispose();
            _connectionManager.Dispose();
        }
    }
}