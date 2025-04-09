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
        readonly IDictionary<string, IDictionary<string, IRunTool>> _runners;

        public ToolDispatcher(ILogger<ToolDispatcher> logger, IDictionary<string, IDictionary<string, IRunTool>> runners)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogTrace("Ctor.");

            _runners = runners ?? throw new ArgumentNullException(nameof(runners));

            _logger.LogTrace("Registered commands [{0}]:", _runners.Count);
            foreach (var classKeyValue in _runners)
            {
                foreach (var methodKeyValue in classKeyValue.Value)
                    _logger.LogTrace("Command: {0}.{1}", classKeyValue.Key, methodKeyValue.Key);
            }
        }

        /// <summary>
        /// Executes a command based on the provided CommandData.
        /// </summary>
        /// <param name="data">The CommandData containing the command name and parameters.</param>
        public IResponseData Dispatch(IRequestTool data)
        {
            if (data == null)
                return ResponseData.Error("Command data is null.")
                    .Log(_logger);

            if (data.Class == null)
                return ResponseData.Error("Command.Class is null.")
                    .Log(_logger);

            if (data.Method == null)
                return ResponseData.Error("Command.Method is null.")
                    .Log(_logger);

            if (!_runners.TryGetValue(data.Class, out var commandGroup))
                return ResponseData.Error($"Command with Class '{data.Class}' not found.")
                    .Log(_logger);

            if (!commandGroup.TryGetValue(data.Method, out var command))
                return ResponseData.Error($"Command with Method '{data.Method}' not found.")
                    .Log(_logger);

            try
            {
                var message = data.Parameters == null
                    ? $"Executing command '{data.Method}' with no parameters."
                    : $"Executing command '{data.Method}' with parameters[{data.Parameters.Count}]:\n{string.Join(",\n", data.Parameters)}";
                _logger.LogInformation(message);

                // Execute the command with the parameters from CommandData
                return command.Run(data.Parameters)
                    .Log(_logger);
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                return ResponseData.Error($"Failed to execute command '{data.Method}'. Exception: {ex}")
                    .Log(_logger, ex);
            }
        }

        public void Dispose()
        {
            _runners.Clear();
        }
    }
}