#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.UnityMCP.Common
{
    public static class LogLevelExtensions
    {
        public static bool IsEnabled(this LogLevel logLevel, LogLevel targetLogLevel)
            => logLevel <= targetLogLevel;

        public static string ToString(this LogLevel logLevel) => logLevel switch
        {
            LogLevel.Trace => "Trace",
            LogLevel.Debug => "Debug",
            LogLevel.Information => "Information",
            LogLevel.Warning => "Warning",
            LogLevel.Error => "Error",
            LogLevel.Critical => "Critical",
            _ => "None"
        };
    }
}