using System;
using com.IvanMurzak.UnityMCP.Common;
using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.UnityMCP.Editor
{
    internal class UnityLogger : ILogger
    {
        readonly string _categoryName;

        public UnityLogger(string categoryName)
        {
            _categoryName = categoryName.Contains('.')
                ? categoryName.Substring(categoryName.LastIndexOf('.') + 1)
                : categoryName;
        }

        public IDisposable BeginScope<TState>(TState state) => null!;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter == null) throw new ArgumentNullException(nameof(formatter));
            if (state == null) throw new ArgumentNullException(nameof(state));

            var message = $"{Consts.Log.Tag} {Consts.Log.Color.CategoryStart}{_categoryName}{Consts.Log.Color.CategoryEnd} {Consts.Log.Color.LevelStart}[{logLevel}]{Consts.Log.Color.LevelEnd} {formatter(state, exception)}";
            switch (logLevel)
            {
                case LogLevel.Critical:
                case LogLevel.Error:
                    UnityEngine.Debug.LogError(message);
                    if (exception != null)
                        UnityEngine.Debug.LogException(exception);
                    break;

                case LogLevel.Warning:
                    UnityEngine.Debug.LogWarning(message);
                    break;

                default:
                    UnityEngine.Debug.Log(message);
                    break;
            }
        }
    }
    internal class UnityLoggerProvider : ILoggerProvider
    {
        public void Dispose() { /* No resources to dispose of */ }
        ILogger ILoggerProvider.CreateLogger(string categoryName) => new UnityLogger(categoryName);
    }
}