using System;
using Lib.Scryfall.Ingestion.Apis.Dashboard;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Dashboard;

internal sealed class SilentDashboard : IIngestionDashboard
{
    private readonly ILogger _logger;
    private int _lastSetMilestone;
    private int _lastCardMilestone;

    public SilentDashboard(ILogger logger)
    {
        _logger = logger;
    }

    public void SetStartTime()
    {
        _logger.LogBulkIngestionStarted();
    }

    public void UpdateSetProgress(int current, int total, string name)
    {
        // Log every 50 sets or at completion
        if (current == total || current - _lastSetMilestone >= 50)
        {
            _logger.LogSetProgress(current, total);
            _lastSetMilestone = current;
        }
    }

    public void UpdateCardProgress(int current, int total, string name)
    {
        // Log every 5000 cards or at completion
        if (current == total || current - _lastCardMilestone >= 5000)
        {
            _logger.LogCardProgress(current, total);
            _lastCardMilestone = current;
        }
    }

    public void UpdateArtistCount(int count)
    {
        // Only log at specific milestones
        if (count % 500 == 0)
        {
            _logger.LogArtistDiscovered(count);
        }
    }

    public void UpdateRulingCount(int count)
    {
        // Only log at specific milestones
        if (count % 1000 == 0)
        {
            _logger.LogRulingsProcessed(count);
        }
    }

    public void AddCompletedSet(string name)
    {
        _logger.LogSetCompleted(name);
    }

    public void UpdateMemoryUsage()
    {
        // Silent mode doesn't track memory
    }

    public void Refresh()
    {
        // No-op in silent mode
    }

    public void Complete(string message)
    {
        _logger.LogIngestionComplete(message);
    }

    // ILogger implementation - delegate to underlying logger
    public IDisposable BeginScope<TState>(TState state) => _logger.BeginScope(state);

    public bool IsEnabled(LogLevel logLevel) => _logger.IsEnabled(logLevel);

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        _logger.Log(logLevel, eventId, state, exception, formatter);
    }
}

internal static partial class SilentDashboardLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Bulk ingestion started")]
    public static partial void LogBulkIngestionStarted(this ILogger logger);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Processed {Current}/{Total} sets")]
    public static partial void LogSetProgress(this ILogger logger, int current, int total);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Processed {Current}/{Total} cards")]
    public static partial void LogCardProgress(this ILogger logger, int current, int total);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Discovered {Count} unique artists")]
    public static partial void LogArtistDiscovered(this ILogger logger, int count);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Processed {Count} ruling groups")]
    public static partial void LogRulingsProcessed(this ILogger logger, int count);

    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Completed set: {SetName}")]
    public static partial void LogSetCompleted(this ILogger logger, string setName);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "{Message}")]
    public static partial void LogIngestionComplete(this ILogger logger, string message);
}