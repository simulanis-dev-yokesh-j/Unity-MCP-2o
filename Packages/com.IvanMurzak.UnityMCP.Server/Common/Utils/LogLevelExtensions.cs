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