using System;
using Lib.Scryfall.Ingestion.Apis.Dashboard;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Dashboard;

public sealed class DashboardFactory : IDashboardFactory
{
    public IIngestionDashboard Create(ILogger logger)
    {
        // For now, always use ConsoleDashboard
        logger.LogInteractiveModeEnabled();
        return new ConsoleDashboard();
    }
}

internal static partial class DashboardFactoryLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Running in interactive mode - console dashboard enabled")]
    public static partial void LogInteractiveModeEnabled(this ILogger logger);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Running in pipeline mode - structured logging enabled")]
    public static partial void LogPipelineModeEnabled(this ILogger logger);
}