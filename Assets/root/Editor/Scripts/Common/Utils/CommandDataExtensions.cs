#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.Collections.Generic;
using com.IvanMurzak.UnityMCP.Common.Data;

namespace com.IvanMurzak.UnityMCP.Common
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
        public static IDataPackage Build(this ICommandData data) => new DataPackage()
        {
            Command = data as CommandData,
        };
    }
}