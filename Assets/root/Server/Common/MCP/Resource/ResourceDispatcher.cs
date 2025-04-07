#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Collections.Generic;
using com.IvanMurzak.Unity.MCP.Common.Data;
using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public partial class ResourceDispatcher : IResourceDispatcher
    {
        readonly ILogger<ResourceDispatcher> _logger;
        readonly IDictionary<string, ICommand> _resources;

        public ResourceDispatcher(ILogger<ResourceDispatcher> logger, IDictionary<string, ICommand> commands)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogTrace("Ctor.");

            _resources = commands ?? throw new ArgumentNullException(nameof(commands));

            _logger.LogTrace("Registered resources [{0}]:", _resources.Count);
            foreach (var keyValuePair in _resources)
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

            if (!_resources.TryGetValue(data.Uri, out var resource))
                return ResponseData.Error($"Resource with Uri '{data.Uri}' not found.")
                    .Log(_logger);

            try
            {
                _logger.LogInformation("Executing resource '{0}'.", data.Uri);

                // Execute the resource with the parameters from Uri
                // TODO: Implement the logic to execute the resource with parameters
                return resource.Execute(data.Uri)
                    .Log(_logger);
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                return ResponseData.Error($"Failed to execute resource '{data.Uri}'. Exception: {ex}")
                    .Log(_logger, ex);
            }
        }

        public void Dispose()
        {
            _resources.Clear();
        }
    }
}