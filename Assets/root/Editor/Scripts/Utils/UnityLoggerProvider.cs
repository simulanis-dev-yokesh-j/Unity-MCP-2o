using System;
using Microsoft.Extensions.Logging;
using UnityEngine;

internal class UnityLoggerProvider : ILoggerProvider
{
    public void Dispose()
    {
        // No resources to dispose of
    }

    Microsoft.Extensions.Logging.ILogger ILoggerProvider.CreateLogger(string categoryName)
        => new UnityLogger(categoryName);
}

internal class UnityLogger : Microsoft.Extensions.Logging.ILogger
{
    private readonly string _categoryName;

    public UnityLogger(string categoryName)
    {
        _categoryName = categoryName;
    }

    public IDisposable BeginScope<TState>(TState state) => null!;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (formatter == null) throw new ArgumentNullException(nameof(formatter));
        if (state == null) throw new ArgumentNullException(nameof(state));

        var message = formatter(state, exception);
        Debug.Log($"[{logLevel}] {_categoryName}: {message}");
    }
}