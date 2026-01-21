using System;
using System.Net.Http;
using System.Threading.Tasks;
using Cli.MtgDiscovery.PriceUpdate.Configuration;
using Cli.MtgDiscovery.PriceUpdate.ErrorTracking;
using Cli.MtgDiscovery.PriceUpdate.ManaPool;
using Cli.MtgDiscovery.PriceUpdate.Mapping;
using Cli.MtgDiscovery.PriceUpdate.Orchestration;
using Cli.MtgDiscovery.PriceUpdate.PriceChangeLogging;
using Cli.MtgDiscovery.PriceUpdate.Progress;
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
            _ = builder.SetMinimumLevel(LogLevel.Information);
        });

        ILogger logger = loggerFactory.CreateLogger<PriceUpdateApplication>();

        try
        {
            logger.LogInformation("Starting Price Update Tool");

            PriceUpdateConfiguration config = Configuration
                .GetSection("PriceUpdateConfiguration")
                .Get<PriceUpdateConfiguration>();

            if (config is null)
            {
                logger.LogError("PriceUpdateConfiguration is missing");
                throw new InvalidOperationException("PriceUpdateConfiguration must be configured");
            }

            logger.LogInformation("Configuration loaded successfully");
            logger.LogInformation("ManaPool API URL: {Url}", config.ManaPoolApiUrl);
            logger.LogInformation("Error Output Path: {Path}", config.ErrorOutputPath);
            logger.LogInformation("Price Change Log Path: {Path}", config.PriceChangeLogPath);
            logger.LogInformation("Test Mode: {TestMode}", config.TestMode);

            if (config.TestMode)
            {
                logger.LogInformation("Test Card Limit: {Limit}", config.TestCardLimit);
            }

            logger.LogInformation("Creating ManaPool API client...");
            using HttpClient httpClient = new();
            IManaPoolApiClient apiClient = new ManaPoolApiClient(
                loggerFactory.CreateLogger<ManaPoolApiClient>(),
                httpClient,
                config);

            logger.LogInformation("Creating price mapper...");
            IManaPoolToPricesMapper priceMapper = new ManaPoolToPricesMapper();

            logger.LogInformation("Creating container updaters...");
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

            logger.LogInformation("Creating loggers...");
            using IPriceUpdateErrorLogger errorLogger = new CsvPriceUpdateErrorLogger(
                loggerFactory.CreateLogger<CsvPriceUpdateErrorLogger>(),
                config);

            using IPriceChangeLogger priceChangeLogger = new CsvPriceChangeLogger(
                loggerFactory.CreateLogger<CsvPriceChangeLogger>(),
                config);

            IPriceUpdateProgressTracker progressTracker = new PriceUpdateProgressTracker(
                loggerFactory.CreateLogger<PriceUpdateProgressTracker>());

            logger.LogInformation("Creating orchestrator...");
            IPriceUpdateOrchestrator orchestrator = new PriceUpdateOrchestrator(
                loggerFactory.CreateLogger<PriceUpdateOrchestrator>(),
                config,
                apiClient,
                cardItemsUpdater,
                setCardsUpdater,
                artistCardsUpdater,
                cardsByNameUpdater,
                errorLogger,
                priceChangeLogger,
                progressTracker);

            logger.LogInformation("Starting price update execution...");
            PriceUpdateResult result = await orchestrator.ExecuteAsync().ConfigureAwait(false);

            logger.LogInformation(
                "Price update completed. Total: {Total}, Updated: {Updated}, Skipped: {Skipped}, Errors: {Errors}, Total RU: {RU:F2}",
                result.TotalCards,
                result.UpdatedCards,
                result.SkippedCards,
                result.Errors,
                result.TotalRuConsumed);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fatal error during price update execution");
            throw;
        }
    }
}
