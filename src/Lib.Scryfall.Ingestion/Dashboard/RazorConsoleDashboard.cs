using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Apis.Dashboard;
using Lib.Scryfall.Ingestion.Dashboard.RazorUI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RazorConsole.Core;

namespace Lib.Scryfall.Ingestion.Dashboard;

internal sealed partial class RazorConsoleDashboard : IIngestionDashboard, IDisposable
{
    private readonly DashboardState _state;
    private readonly ILogger _fallbackLogger;

    public RazorConsoleDashboard(ILogger fallbackLogger)
    {
        _fallbackLogger = fallbackLogger;
        _state = new DashboardState();

        // Log that RazorConsole is being initialized
        LogRazorDashboardInitialized(fallbackLogger);
    }

    public async Task RunUiAsync()
    {
        IHostBuilder hostBuilder = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton(_state);
            })
            .UseRazorConsole<IngestionDashboard>();

        IHost host = hostBuilder.Build();
        await host.RunAsync().ConfigureAwait(false);
    }

    public void UpdateProgress(string type, int current, int total, string action, string item) =>
        _state.UpdateProgress(type, current, total, action, item);

    public void UpdateSetProgress(int current, int total, string name) =>
        _state.UpdateSetProgress(current, total, name);

    public void UpdateCardProgress(int current, int total, string name) =>
        _state.UpdateCardProgress(current, total, name);

    public void UpdateRulingProgress(int current, int total, string name) =>
        _state.UpdateRulingProgress(current, total, name);

    public void UpdateTrigramProgress(int current, int total, string name) =>
        _state.UpdateTrigramProgress(current, total, name);

    public void AddCompletedSet(string name)
    {
        // No-op for RazorConsole - uses UpdateCompletedCount instead
    }

    public void UpdateMemoryUsage() => _state.UpdateMemoryUsage();

    public void UpdateCompletedCount(string type, int count) =>
        _state.UpdateCompletedCount(type, count);

    public void SetStartTime() => _state.StartTimer();

    public void Refresh()
    {
        // No-op - RazorConsole handles refreshing automatically
    }

    public void Complete(string message) => _state.MarkComplete(message);

    public CancellationToken GetCancellationToken() => _state.GetCancellationToken();

    public IDisposable BeginScope<TState>(TState state) where TState : notnull =>
        _fallbackLogger.BeginScope(state);

    public bool IsEnabled(LogLevel logLevel) => _fallbackLogger.IsEnabled(logLevel);

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (IsEnabled(logLevel) is false) return;

        string message = formatter(state, exception);
        if (string.IsNullOrWhiteSpace(message)) return;

        _state.AddLog($"[{logLevel}] {message}");
        // Don't forward to fallback logger to avoid console output interference with RazorConsole
    }

    public void Dispose() => _state.Dispose();

    [LoggerMessage(Level = LogLevel.Information, Message = "{message}")]
    private static partial void LogCompletionMessage(ILogger logger, string message);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Failed to render dashboard")]
    private static partial void LogRenderFailure(ILogger logger, Exception ex);

    [LoggerMessage(Level = LogLevel.Information, Message = "RazorConsoleDashboard initialized - interactive UI mode")]
    private static partial void LogRazorDashboardInitialized(ILogger logger);

    [LoggerMessage(Level = LogLevel.Information, Message = "RazorConsoleDashboard.RunUiAsync() starting")]
    private static partial void LogRunUiStarting(ILogger logger);
}
