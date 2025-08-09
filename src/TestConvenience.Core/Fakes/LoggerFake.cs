using System;
using Microsoft.Extensions.Logging;

namespace TestConvenience.Core.Fakes;

/// <summary>
/// Fake for the ILogger interface. Only allows count checking.
/// </summary>
public sealed class LoggerFake : ILogger
{
    public int LogInvokeCount { get; private set; }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        LogInvokeCount++;
    }

    public bool IsEnabled(LogLevel logLevel) => true;

    public IDisposable BeginScope<TState>(TState state) => throw new NotImplementedException();
}
