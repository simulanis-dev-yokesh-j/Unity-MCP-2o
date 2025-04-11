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
        readonly IDictionary<string, IRunResource> _runners;

        public ResourceDispatcher(ILogger<ResourceDispatcher> logger, IDictionary<string, IRunResource> runners)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogTrace("Ctor.");

            _runners = runners ?? throw new ArgumentNullException(nameof(runners));

            if (_logger.IsEnabled(LogLevel.Trace))
            {
                _logger.LogTrace("Registered resources [{0}]:", _runners.Count);
                foreach (var keyValuePair in _runners)
                    _logger.LogTrace("Resource: {0}", keyValuePair.Key);
            }
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

            var runner = FindRunner(data.Uri, out var uriTemplate)?.RunGetContent;
            if (runner == null || uriTemplate == null)
                throw new ArgumentException($"No route matches the URI: {data.Uri}");

            _logger.LogInformation("Executing resource '{0}'.", data.Uri);

            // Execute the resource with the parameters from Uri
            // TODO: Implement the logic to execute the resource with parameters
            // TODO: parse variables from Uri
            var parameters = ParseUriParameters(uriTemplate!, data.Uri);
            PrintParameters(parameters);
            return runner.Run(parameters).Result.ToArray();
        }

        public IResponseListResource[] Dispatch(IRequestListResources data)
            => _runners.Values
                .SelectMany(resource => resource.RunListContext.Run().Result)
                .ToArray();

        public IResponseResourceTemplate[] Dispatch(IRequestListResourceTemplates data)
            => _runners.Values
                .Select(resource => new ResponseResourceTemplate(resource.Route, resource.Name, resource.Description, resource.MimeType))
                .ToArray();

        IRunResource? FindRunner(string uri, out string? uriTemplate)
        {
            foreach (var route in _runners)
            {
                if (IsMatch(route.Key, uri))
                {
                    uriTemplate = route.Key;
                    return route.Value;
                }
            }
            uriTemplate = null;
            return null;
        }

        bool IsMatch(string uriTemplate, string uri)
        {
            // Convert pattern to regex
            var regexPattern = "^" + Regex.Replace(uriTemplate, @"\{(\w+)\}", @"(?<$1>[^/]+)") + "(?:/.*)?$";

            return Regex.IsMatch(uri, regexPattern);
        }

        IDictionary<string, object?> ParseUriParameters(string pattern, string uri)
        {
            var parameters = new Dictionary<string, object?>()
            {
                { "uri", uri }
            };

            // Convert pattern to regex
            var regexPattern = "^" + Regex.Replace(pattern, @"\{(\w+)\}", @"(?<$1>.+)") + "(?:/.*)?$";

            var regex = new Regex(regexPattern);
            var match = regex.Match(uri);

            if (match.Success)
            {
                foreach (var groupName in regex.GetGroupNames())
                {
                    if (groupName != "0") // Skip the entire match group
                    {
                        parameters[groupName] = match.Groups[groupName].Value;
                    }
                }
            }

            return parameters;
        }

        void PrintParameters(IDictionary<string, object?> parameters)
        {
            if (!_logger.IsEnabled(LogLevel.Debug))
                return;

            var parameterLogs = string.Join(Environment.NewLine, parameters.Select(kvp => $"{kvp.Key} = {kvp.Value ?? "null"}"));
            _logger.LogDebug("Parsed Parameters [{0}]:\n{1}", parameters.Count, parameterLogs);
        }

        public void Dispose()
        {
            _runners.Clear();
        }
    }
}