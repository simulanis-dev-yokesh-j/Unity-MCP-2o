#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.Unity.MCP.Common.Data
{
    public static class ResponseResourceContentExtensions
    {
        public static List<IResponseResourceContent> Log(this List<IResponseResourceContent> target, ILogger logger, Exception? ex = null)
        {
            if (!logger.IsEnabled(LogLevel.Information))
                return target;

            foreach (var item in target)
                logger.LogInformation(ex, "Resource: {0}", item.uri);

            return target;
        }


        public static IResponseData<List<IResponseResourceContent>> Pack(this List<IResponseResourceContent> target, string requestId, string? message = null)
            => ResponseData<List<IResponseResourceContent>>.Success(requestId, message ?? "List Tool execution completed.")
                .SetData(target);
    }
}