using System;
using System.Collections.Generic;
using com.IvanMurzak.UnityMCP.Common.API;
using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.UnityMCP.Common
{
    public partial class CommandDispatcher : ICommandDispatcher
    {
        readonly ILogger<CommandDispatcher> _logger;
        readonly Dictionary<string, Command> _commands;

        public CommandDispatcher(ILogger<CommandDispatcher> logger, Dictionary<string, Command> commands)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _commands = commands ?? throw new ArgumentNullException(nameof(commands));
        }

        /// <summary>
        /// Executes a command based on the provided CommandData.
        /// </summary>
        /// <param name="data">The CommandData containing the command name and parameters.</param>
        public ICommandResponseData Dispatch(ICommandData data)
        {
            if (data == null)
                return CommandResponseData.Error("Command data is null.")
                    .Log(_logger);

            if (!_commands.TryGetValue(data.Name, out var command))
                return CommandResponseData.Error($"Command with name '{data.Name}' not found.")
                    .Log(_logger);

            try
            {
                // Execute the command with the parameters from CommandData
                _logger.LogInformation($"Executing command '{data.Name}' with parameters[{data.Parameters.Count}]:\n{string.Join(",\n", data.Parameters)}");
                return command.Execute(data.Parameters)
                    .Log(_logger);
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                return CommandResponseData.Error($"Failed to execute command '{data.Name}'. Exception: {ex}")
                    .Log(_logger);
            }
        }

        public void Dispose()
        {
            _commands.Clear();
        }
    }
}