namespace com.IvanMurzak.Unity.MCP
{
    public enum LogLevel
    {
        Trace = -1, // show all messages
        Log = 0,  // show only Log, Warning, Error, Exception messages
        Warning = 1,  // show only Warning, Error, Exception messages
        Error = 2,  // show only Error, Exception messages
        Exception = 3,  // show only Exception messages
        None = 4   // show no messages
    }
    public static class LogLevelEx
    {
        /// <summary>
        /// Check if the LogLevel is active
        /// If it is active the related message will be shown in the console
        /// </summary>
        public static bool IsActive(this LogLevel logLevel, LogLevel level) => logLevel <= level;
    }
}