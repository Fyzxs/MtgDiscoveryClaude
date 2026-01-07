using System;
using System.Net.Http;
using System.Threading.Tasks;
using Cli.MtgDiscovery.PriceUpdate.Configuration;
using Cli.MtgDiscovery.PriceUpdate.Dashboard;
using Cli.MtgDiscovery.PriceUpdate.ErrorTracking;
using Cli.MtgDiscovery.PriceUpdate.ManaPool;
using Cli.MtgDiscovery.PriceUpdate.Mapping;
using Cli.MtgDiscovery.PriceUpdate.Orchestration;
using Cli.MtgDiscovery.PriceUpdate.PriceChangeLogging;
using Cli.MtgDiscovery.PriceUpdate.Updaters;
using Example.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Cli.MtgDiscovery.PriceUpdate;

internal sealed class PriceUpdateApplication : ExampleApplication
{
    protected override async Task Execute()
    {
        using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            _ = builder.AddConsole();
            _ = builder.SetMinimumLevel(LogLevel.Warning);
        });

        ILogger logger = loggerFactory.CreateLogger<PriceUpdateApplication>();

        PriceUpdateConfiguration config = Configuration
            .GetSection("PriceUpdateConfiguration")
            .Get<PriceUpdateConfiguration>();

        if (config is null)
        {
            logger.LogError("PriceUpdateConfiguration is missing");
            throw new InvalidOperationException("PriceUpdateConfiguration must be configured");
        }

        using RazorConsolePriceUpdateDashboard dashboard = new();

        Task ingestionTask = Task.Run(async () =>
        {
            try
            {
                await RunPriceUpdateAsync(config, dashboard, loggerFactory).ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Swallow exception to pass to UI
            catch (Exception ex)
#pragma warning restore CA1031
            {
                logger.LogError(ex, "Fatal error during price update");
                dashboard.MarkComplete($"Failed: {ex.Message}");
            }
        });

        await dashboard.RunUiAsync().ConfigureAwait(false);
        await ingestionTask.ConfigureAwait(false);
    }

    private static async Task RunPriceUpdateAsync(
        PriceUpdateConfiguration config,
        IPriceUpdateDashboard dashboard,
        ILoggerFactory loggerFactory)
    {
        using HttpClient httpClient = new()
        {
            Timeout = TimeSpan.FromMinutes(5)
        };

        IManaPoolApiClient apiClient = new ManaPoolApiClient(
            loggerFactory.CreateLogger<ManaPoolApiClient>(),
            httpClient,
            config);

        IManaPoolToPricesMapper priceMapper = new ManaPoolToPricesMapper();

        IPriceUpdater cardItemsUpdater = new CardItemsPriceUpdater(
            loggerFactory.CreateLogger<CardItemsPriceUpdater>(),
            priceMapper);

        IPriceUpdater setCardsUpdater = new SetCardsPriceUpdater(
            loggerFactory.CreateLogger<SetCardsPriceUpdater>(),
            priceMapper);

        IPriceUpdater artistCardsUpdater = new ArtistCardsPriceUpdater(
            loggerFactory.CreateLogger<ArtistCardsPriceUpdater>(),
            priceMapper);

        IPriceUpdater cardsByNameUpdater = new CardsByNamePriceUpdater(
            loggerFactory.CreateLogger<CardsByNamePriceUpdater>(),
            priceMapper);

        using IPriceUpdateErrorLogger errorLogger = new CsvPriceUpdateErrorLogger(
            loggerFactory.CreateLogger<CsvPriceUpdateErrorLogger>(),
            config);

        using IPriceChangeLogger priceChangeLogger = new CsvPriceChangeLogger(
            loggerFactory.CreateLogger<CsvPriceChangeLogger>(),
            config);

        IPriceUpdateOrchestrator orchestrator = new PriceUpdateOrchestrator(
            config,
            apiClient,
            cardItemsUpdater,
            setCardsUpdater,
            artistCardsUpdater,
            cardsByNameUpdater,
            errorLogger,
            priceChangeLogger,
            dashboard);

        _ = await orchestrator.ExecuteAsync().ConfigureAwait(false);
    }
}
