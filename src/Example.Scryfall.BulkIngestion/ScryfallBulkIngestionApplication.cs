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

        // Check if this is a RazorConsole dashboard
        if (dashboard is Lib.Scryfall.Ingestion.Dashboard.RazorConsoleDashboard razorDashboard)
        {
            // For RazorConsole: Run ingestion in background, UI on main thread
            Task ingestionTask = Task.Run(async () =>
            {
                try
                {
                    IBulkIngestionService bulkIngestionService = new BulkIngestionService(dashboard, dashboard);
                    await bulkIngestionService.IngestBulkDataAsync().ConfigureAwait(false);
                }
#pragma warning disable CA1031 // Swallow exception to pass to UI
                catch (Exception ex)
#pragma warning restore CA1031
                {
                    logger.LogError(ex, "Fatal error during bulk ingestion");
                    dashboard.Complete($"Ingestion failed: {ex.Message}");
                }
            });

            // Run RazorConsole UI on main thread (blocking)
            await razorDashboard.RunUIAsync().ConfigureAwait(false);

            // Wait for ingestion to complete
            await ingestionTask.ConfigureAwait(false);
        }
        else
        {
            // For regular dashboard: Run normally
            try
            {
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
