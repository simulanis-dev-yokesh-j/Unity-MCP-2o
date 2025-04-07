#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using com.IvanMurzak.Unity.MCP.Common.Data;
using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public partial class ResourceDispatcher : IResourceDispatcher
    {
        readonly ILogger<ResourceDispatcher> _logger;
        readonly IDictionary<string, IResourceParams> _commands;

        public ResourceDispatcher(ILogger<ResourceDispatcher> logger, IDictionary<string, IResourceParams> commands)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogTrace("Ctor.");

            _commands = commands ?? throw new ArgumentNullException(nameof(commands));

            _logger.LogTrace("Registered resources [{0}]:", _commands.Count);
            foreach (var keyValuePair in _commands)
                _logger.LogTrace("Resource: {0}", keyValuePair.Key);
        }

        /// <summary>
        /// Get resources based on the provided Uri.
        /// </summary>
        /// <param name="data">The ResourceData containing the resource Uri and parameters.</param>
        /// <returns>ResponseData containing the result of the resource retrieval.</returns>
        public IResponseData Dispatch(IRequestResourceData data)
        {
            if (data == null)
                return ResponseData.Error("Resource data is null.")
                    .Log(_logger);

            if (data.Uri == null)
                return ResponseData.Error("Resource.Uri is null.")
                    .Log(_logger);


            var command = FindCommand(data.Uri)?.Command;
            if (command == null)
                return ResponseData.Error($"No route matches the URI: {data.Uri}")
                    .Log(_logger);

            try
            {
                _logger.LogInformation("Executing resource '{0}'.", data.Uri);

                // Execute the resource with the parameters from Uri
                // TODO: Implement the logic to execute the resource with parameters
                // TODO: parse variables from Uri
                var parameters = ParseUriParameters(data.Uri);
                return command.Execute(parameters)
                    .Log(_logger);
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                return ResponseData.Error($"Failed to execute resource '{data.Uri}'. Exception: {ex}")
                    .Log(_logger, ex);
            }
        }

        IResourceParams? FindCommand(string uri)
        {
            foreach (var route in _commands)
            {
                if (IsMatch(route.Key, uri))
                    return route.Value;
            }
            return null;
        }

        bool IsMatch(string pattern, string uri)
        {
            // Convert pattern to regex
            var regexPattern = "^" + Regex.Escape(pattern)
                .Replace("\\*", ".*")
                .Replace("\\{.*?\\}", "[^/]+") + "$";

            return Regex.IsMatch(uri, regexPattern);
        }

        Dictionary<string, object?> ParseUriParameters(string uri)
        {
            var parameters = new Dictionary<string, object?>();
            var regex = new Regex(@"\{(?<name>[^}]+)\}");
            var matches = regex.Matches(uri);

            foreach (Match match in matches)
            {
                var name = match.Groups["name"].Value;
                if (!parameters.ContainsKey(name))
                {
                    parameters[name] = null; // Initialize with null or default value
                }
            }

            return parameters;
        }

        public void Dispose()
        {
            _commands.Clear();
        }
    }
}