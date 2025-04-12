#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.Unity.MCP.Common.Data
{
    public static class ResponseListToolExtensions
    {
        public static List<IResponseListTool> Log(this List<IResponseListTool> response, ILogger logger, Exception? ex = null)
        {
            if (!logger.IsEnabled(LogLevel.Information))
                return response;

            foreach (var item in response)
                logger.LogInformation(ex, JsonUtils.ToJson(item));

            return response;
        }

        public static IResponseData<List<IResponseListTool>> Pack(this List<IResponseListTool> response, string requestId, string? message = null)
            => ResponseData<List<IResponseListTool>>.Success(requestId, message ?? "List Tool execution completed.")
                .SetData(response);
    }
}