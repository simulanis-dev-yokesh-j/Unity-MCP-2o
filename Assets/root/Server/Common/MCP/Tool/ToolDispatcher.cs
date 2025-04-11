#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Collections.Generic;
using com.IvanMurzak.Unity.MCP.Common.Data;
using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public partial class ToolDispatcher : IToolDispatcher
    {
        readonly ILogger<ToolDispatcher> _logger;
        readonly IDictionary<string, IRunTool> _runners;

        public ToolDispatcher(ILogger<ToolDispatcher> logger, IDictionary<string, IRunTool> runners)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogTrace("Ctor.");

            _runners = runners ?? throw new ArgumentNullException(nameof(runners));

            if (_logger.IsEnabled(LogLevel.Trace))
            {
                _logger.LogTrace("Registered commands [{0}]:", _runners.Count);
                foreach (var methodKeyValue in _runners)
                    _logger.LogTrace("Command: {1}", methodKeyValue.Key);
            }
        }

        /// <summary>
        /// Executes a command based on the provided CommandData.
        /// </summary>
        /// <param name="data">The CommandData containing the command name and parameters.</param>
        public IResponseData Dispatch(IRequestCallTool data)
        {
            if (data == null)
                return ResponseData.Error("Tool data is null.")
                    .Log(_logger);

            if (string.IsNullOrEmpty(data.Name))
                return ResponseData.Error("Tool.Name is null.")
                    .Log(_logger);

            if (!_runners.TryGetValue(data.Name, out var runner))
                return ResponseData.Error($"Tool with Name '{data.Name}' not found.")
                    .Log(_logger);

            try
            {
                var message = data.Arguments == null
                    ? $"Run tool '{data.Name}' with no parameters."
                    : $"Run tool '{data.Name}' with parameters[{data.Arguments.Count}]:\n{string.Join(",\n", data.Arguments)}";
                _logger.LogInformation(message);

                // Execute the command with the parameters from CommandData
                return runner.Run(data.Arguments)
                    .Log(_logger);
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                return ResponseData.Error($"Failed to run tool '{data.Name}'. Exception: {ex}")
                    .Log(_logger, ex);
            }
        }

        public void Dispose()
        {
            _runners.Clear();
        }
    }
}