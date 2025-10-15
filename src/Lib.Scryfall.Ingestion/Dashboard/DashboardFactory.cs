using Lib.Scryfall.Ingestion.Apis.Configuration;
using Lib.Scryfall.Ingestion.Apis.Dashboard;
using Lib.Scryfall.Ingestion.Configuration;

namespace Lib.Scryfall.Ingestion.Dashboard;

public sealed class DashboardFactory : IDashboardFactory
{
    public IIngestionDashboard Create(ILogger logger)
    {
        // For now, always use ConsoleDashboard
        logger.LogInteractiveModeEnabled();
        IBulkProcessingConfiguration config = new DefaultBulkProcessingConfiguration();
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
        Message = "Running in pipeline mode - structured logging enabled")]
    public static partial void LogPipelineModeEnabled(this ILogger logger);
}
