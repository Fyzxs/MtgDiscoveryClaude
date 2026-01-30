using Lib.Scryfall.Ingestion.Apis.Configuration;
using Lib.Scryfall.Ingestion.Apis.Dashboard;
using Lib.Scryfall.Ingestion.Configuration;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Dashboard;

public sealed class DashboardFactory : IDashboardFactory
{
    public IIngestionDashboard Create(ILogger logger)
    {
        IBulkProcessingConfiguration config = new ConfigBulkProcessingConfiguration();

        if (config.UseRazorConsole)
        {
            logger.LogRazorConsoleModeEnabled();
            return new RazorConsoleDashboard(logger);
        }

        logger.LogInteractiveModeEnabled();
        return new ConsoleDashboard(config.DashboardRefreshFrequency, config.EnableMemoryThrottling);
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
        Message = "Running in RazorConsole mode - interactive dashboard enabled")]
    public static partial void LogRazorConsoleModeEnabled(this ILogger logger);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Running in pipeline mode - structured logging enabled")]
    public static partial void LogPipelineModeEnabled(this ILogger logger);
}
