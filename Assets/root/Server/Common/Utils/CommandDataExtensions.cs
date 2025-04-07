#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.Collections.Generic;
using com.IvanMurzak.Unity.MCP.Common.Data;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public static class CommandDataExtensions
    {
        public static ICommandData SetName(this ICommandData data, string name)
        {
            data.Method = name;
            return data;
        }
        public static ICommandData SetOrAddParameter(this ICommandData data, string name, object? value)
        {
            data.Parameters ??= new Dictionary<string, object?>();
            data.Parameters[name] = value;
            return data;
        }
        public static IRequestData BuildRequest(this ICommandData data) => new RequestData()
        {
            Command = data as CommandData,
        };
    }
}