#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using com.IvanMurzak.Unity.MCP.Common.Data;
using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public partial class ResourceDispatcher : IResourceDispatcher
    {
        readonly ILogger<ResourceDispatcher> _logger;
        readonly IDictionary<string, IRunResource> _commands;

        public ResourceDispatcher(ILogger<ResourceDispatcher> logger, IDictionary<string, IRunResource> commands)
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
        public IResponseResourceContent[] Dispatch(IRequestResourceContent data)
        {
            if (data == null)
                throw new ArgumentException("Resource data is null.");

            if (data.Uri == null)
                throw new ArgumentException("Resource.Uri is null.");

            var runner = FindRunner(data.Uri)?.RunGetContent;
            if (runner == null)
                throw new ArgumentException($"No route matches the URI: {data.Uri}");

            _logger.LogInformation("Executing resource '{0}'.", data.Uri);

            // Execute the resource with the parameters from Uri
            // TODO: Implement the logic to execute the resource with parameters
            // TODO: parse variables from Uri
            var parameters = ParseUriParameters(data.Uri);
            return runner.Run(parameters);
        }

        public IResponseListResource[] Dispatch(IRequestListResources data)
            => _commands.Values
                .SelectMany(resource => resource.RunListContext.Run())
                .ToArray();

        public IResponseResourceTemplate[] Dispatch(IRequestListResourceTemplates data)
            => _commands.Values
                .Select(resource => new ResponseResourceTemplate(resource.Route, resource.Name, resource.Description, resource.MimeType))
                .ToArray();

        IRunResource? FindRunner(string uri)
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

        IDictionary<string, object?> ParseUriParameters(string uri)
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