using System;
using System.Threading.Tasks;
using Example.Core;
using Lib.Scryfall.Ingestion.Apis.Dashboard;
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
            // Demo the dashboard with simulated progress
            await DemoDashboard(dashboard).ConfigureAwait(false);

            //IBulkIngestionService bulkIngestionService = new BulkIngestionService(logger);
            //await bulkIngestionService.IngestBulkDataAsync().ConfigureAwait(false);

            dashboard.Complete("Bulk ingestion completed successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fatal error during bulk ingestion");
            dashboard.Complete($"Ingestion failed: {ex.Message}");
            throw;
        }
    }

    private static async Task DemoDashboard(IIngestionDashboard dashboard)
    {
        dashboard.SetStartTime();

        // Simulate processing sets
        string[] setNames = [
            "Innistrad: Midnight Hunt",
            "Innistrad: Crimson Vow",
            "Kamigawa: Neon Dynasty",
            "Streets of New Capenna",
            "Dominaria United",
            "The Brothers' War",
            "Phyrexia: All Will Be One",
            "March of the Machine"
        ];

        for (int i = 0; i < setNames.Length; i++)
        {
            dashboard.UpdateSetProgress(i + 1, setNames.Length, setNames[i]);

            // Simulate processing cards in this set
            for (int card = 0; card < 250; card += 10)
            {
                dashboard.UpdateCardProgress(i * 250 + card, setNames.Length * 250, $"Card {card} from {setNames[i]}");
                dashboard.UpdateArtistCount((i + 1) * 50 + card / 10);
                dashboard.UpdateRulingCount((i + 1) * 30 + card / 20);
                dashboard.UpdateMemoryUsage();
                dashboard.Refresh();

                await Task.Delay(50).ConfigureAwait(false); // Simulate work
            }

            dashboard.AddCompletedSet(setNames[i]);
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
