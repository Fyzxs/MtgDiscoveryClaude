using System;
using Lib.Scryfall.Ingestion.Apis.Dashboard;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Dashboard;

internal sealed class SilentDashboard : IIngestionDashboard
{
    private readonly ILogger _logger;
    private int _lastSetMilestone;
    private int _lastCardMilestone;
    private int _lastTrigramMilestone;

    public SilentDashboard(ILogger logger)
    {
        _logger = logger;
    }

    public void SetStartTime()
    {
        _logger.LogBulkIngestionStarted();
    }

    public void UpdateProgress(string type, int current, int total, string action, string item)
    {
        // Log at specific milestones based on type
        if (type == "Sets" && (current == total || current % 50 == 0))
        {
            _logger.LogProgressUpdate(type, current, total, action);
        }
        else if (type == "Cards" && (current == total || current % 5000 == 0))
        {
            _logger.LogProgressUpdate(type, current, total, action);
        }
        else if (type == "Rulings" && (current == total || current % 1000 == 0))
        {
            _logger.LogProgressUpdate(type, current, total, action);
        }
        else if (type == "Card Trigrams" && (current == total || current % 500 == 0))
        {
            _logger.LogProgressUpdate(type, current, total, action);
        }
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

    public void UpdateTrigramProgress(int current, int total, string name)
    {
        // Log every 500 trigrams or at completion
        if (current == total || current - _lastTrigramMilestone >= 500)
        {
            _logger.LogTrigramProgress(current, total);
            _lastTrigramMilestone = current;
        }
    }

    public void UpdateRulingProgress(int current, int total, string name)
    {
        // Only log at specific milestones
        if (current % 1000 == 0 || current == total)
        {
            _logger.LogRulingsProcessed(current, total);
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

    public void UpdateCompletedCount(string type, int count)
    {
        // Log completed counts at specific milestones
        if (count > 0 && (count % 100 == 0 || count == 1))
        {
            _logger.LogCompletedCount(type, count);
        }
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
        Message = "{Action} {Type}: {Current}/{Total}")]
    public static partial void LogProgressUpdate(this ILogger logger, string type, int current, int total, string action);

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
        Message = "Processed {Current}/{Total} trigrams")]
    public static partial void LogTrigramProgress(this ILogger logger, int current, int total);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Processed {Current}/{Total} ruling groups")]
    public static partial void LogRulingsProcessed(this ILogger logger, int current, int total);

    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Completed set: {SetName}")]
    public static partial void LogSetCompleted(this ILogger logger, string setName);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "{Message}")]
    public static partial void LogIngestionComplete(this ILogger logger, string message);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Completed {Type}: {Count}")]
    public static partial void LogCompletedCount(this ILogger logger, string type, int count);
}