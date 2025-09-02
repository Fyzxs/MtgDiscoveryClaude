using System;
using System.Threading.Tasks;
using Example.Core;
using Lib.Scryfall.Ingestion.Apis;
using Lib.Scryfall.Ingestion.Apis.Dashboard;
using Lib.Scryfall.Ingestion.BulkIngestion;
using Lib.Scryfall.Ingestion.Dashboard;
using Microsoft.Extensions.Logging;

namespace Example.Scryfall.BulkIngestion;

public sealed class ScryfallBulkIngestionApplication : ExampleApplication
{
    protected override async Task Execute()
    {
        ILogger logger = GetLogger();

        logger.LogInformation("Starting Scryfall bulk data ingestion");

        // Create dashboard using factory
        IDashboardFactory dashboardFactory = new DashboardFactory();
        IIngestionDashboard dashboard = dashboardFactory.Create(logger);

        try
        {
            // Run the actual bulk ingestion - dashboard implements ILogger
            IBulkIngestionService bulkIngestionService = new BulkIngestionService(dashboard, dashboard);
            await bulkIngestionService.IngestBulkDataAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fatal error during bulk ingestion");
            dashboard.Complete($"Ingestion failed: {ex.Message}");
            throw;
        }
    }

    private ILogger GetLogger()
    {
        using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Information);
            builder.AddConsole();
        });

        return loggerFactory.CreateLogger("BulkIngestion");
    }
}
