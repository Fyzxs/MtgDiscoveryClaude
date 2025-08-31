using System;
using Lib.Scryfall.Ingestion.Apis.Dashboard;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Dashboard;

public sealed class DashboardFactory : IDashboardFactory
{
    public IIngestionDashboard Create(ILogger logger)
    {
        bool isInteractive = Environment.UserInteractive &&
                           !Console.IsOutputRedirected &&
                           Environment.GetEnvironmentVariable("CI") == null &&
                           Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == null;

        if (isInteractive)
        {
            logger.LogInteractiveModeEnabled();
            return new ConsoleDashboard();
        }

        logger.LogPipelineModeEnabled();
        return new SilentDashboard(logger);
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