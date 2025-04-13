#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.Collections.Generic;
using System.Text.Json;
using com.IvanMurzak.Unity.MCP.Common.Data;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public static class RequestCallToolExtensions
    {
        public static IRequestCallTool SetName(this IRequestCallTool data, string name)
        {
            data.Name = name;
            return data;
        }
        public static IRequestCallTool SetOrAddParameter(this IRequestCallTool data, string name, object? value)
        {
            data.Arguments ??= new Dictionary<string, JsonElement>()
            {
                [name] = JsonSerializer.SerializeToElement(value)
            };
            return data;
        }
        // public static IRequestData BuildRequest(this IRequestTool data)
        //     => new RequestData(data as RequestTool ?? throw new System.InvalidOperationException("CommandData is null"));
    }
}