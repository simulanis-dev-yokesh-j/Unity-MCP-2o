#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.Unity.MCP.Common.Data
{
    public static class IResponseResourceTemplateExtensions
    {
        public static List<IResponseResourceTemplate> Log(this List<IResponseResourceTemplate> target, ILogger logger, Exception? ex = null)
        {
            if (!logger.IsEnabled(LogLevel.Information))
                return target;

            foreach (var item in target)
                logger.LogInformation(ex, "Resource Template: {0}", item.uriTemplate);

            return target;
        }

        public static IResponseData<List<IResponseResourceTemplate>> Pack(this List<IResponseResourceTemplate> target, string? message = null)
            => ResponseData<List<IResponseResourceTemplate>>.Success(message ?? "List Tool execution completed.")
                .SetData(target);
    }
}