using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cli.MtgDiscovery.PriceUpdate.Configuration;
using Cli.MtgDiscovery.PriceUpdate.ErrorTracking;
using Cli.MtgDiscovery.PriceUpdate.ManaPool;
using Cli.MtgDiscovery.PriceUpdate.ManaPool.Entities;
using Cli.MtgDiscovery.PriceUpdate.PriceChangeLogging;
using Cli.MtgDiscovery.PriceUpdate.Progress;
using Cli.MtgDiscovery.PriceUpdate.Updaters;
using Microsoft.Extensions.Logging;

namespace Cli.MtgDiscovery.PriceUpdate.Orchestration;

internal sealed class PriceUpdateOrchestrator : IPriceUpdateOrchestrator
{
    private readonly ILogger<PriceUpdateOrchestrator> _logger;
    private readonly PriceUpdateConfiguration _config;
    private readonly IManaPoolApiClient _apiClient;
    private readonly IPriceUpdater _cardItemsUpdater;
    private readonly IPriceUpdater _setCardsUpdater;
    private readonly IPriceUpdater _artistCardsUpdater;
    private readonly IPriceUpdater _cardsByNameUpdater;
    private readonly IPriceUpdateErrorLogger _errorLogger;
    private readonly IPriceChangeLogger _priceChangeLogger;
    private readonly IPriceUpdateProgressTracker _progressTracker;

    public PriceUpdateOrchestrator(
        ILogger<PriceUpdateOrchestrator> logger,
        PriceUpdateConfiguration config,
        IManaPoolApiClient apiClient,
        IPriceUpdater cardItemsUpdater,
        IPriceUpdater setCardsUpdater,
        IPriceUpdater artistCardsUpdater,
        IPriceUpdater cardsByNameUpdater,
        IPriceUpdateErrorLogger errorLogger,
        IPriceChangeLogger priceChangeLogger,
        IPriceUpdateProgressTracker progressTracker)
    {
        _logger = logger;
        _config = config;
        _apiClient = apiClient;
        _cardItemsUpdater = cardItemsUpdater;
        _setCardsUpdater = setCardsUpdater;
        _artistCardsUpdater = artistCardsUpdater;
        _cardsByNameUpdater = cardsByNameUpdater;
        _errorLogger = errorLogger;
        _priceChangeLogger = priceChangeLogger;
        _progressTracker = progressTracker;
    }

    public async Task<PriceUpdateResult> ExecuteAsync()
    {
        _logger.LogInformation("Fetching prices from ManaPool API...");
        IReadOnlyDictionary<string, ManaPoolPriceItem> prices = await _apiClient.FetchAllPricesAsync().ConfigureAwait(false);

        IReadOnlyList<KeyValuePair<string, ManaPoolPriceItem>> priceList = _config.TestMode
            ? prices.Take(_config.TestCardLimit).ToList()
            : prices.ToList();

        _logger.LogInformation("Processing {Count} cards across all containers", priceList.Count);

        int totalOperations = priceList.Count * 4;
        _progressTracker.Initialize(totalOperations);

        int updatedCount = 0;
        int skippedCount = 0;
        int errorCount = 0;
        double totalRu = 0;

        using SemaphoreSlim semaphore = new(_config.MaxConcurrentOperations);

        async Task ProcessContainerAsync(IPriceUpdater updater)
        {
            List<Task> tasks = [];

            foreach (KeyValuePair<string, ManaPoolPriceItem> priceEntry in priceList)
            {
                await semaphore.WaitAsync().ConfigureAwait(false);

                Task task = Task.Run(async () =>
                {
                    try
                    {
                        PriceUpdateItemResult result = await updater
                            .UpdatePriceAsync(priceEntry.Key, priceEntry.Value)
                            .ConfigureAwait(false);

                        ProcessResult(result);
                    }
                    finally
                    {
                        _ = semaphore.Release();
                        _progressTracker.IncrementProgress();
                    }
                });

                tasks.Add(task);
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        void ProcessResult(PriceUpdateItemResult result)
        {
            if (result.HasError)
            {
                _ = Interlocked.Increment(ref errorCount);
                _errorLogger.LogError(new PriceUpdateError
                {
                    ScryfallId = result.ScryfallId,
                    Container = result.Container,
                    ErrorMessage = result.ErrorMessage
                });
            }
            else if (result.WasSkipped)
            {
                _ = Interlocked.Increment(ref skippedCount);
            }
            else if (result.WasUpdated)
            {
                _ = Interlocked.Increment(ref updatedCount);
                _priceChangeLogger.LogChange(new PriceChangeRecord
                {
                    ScryfallId = result.ScryfallId,
                    Container = result.Container,
                    CardName = result.CardName,
                    SetCode = result.SetCode,
                    OldUsd = result.OldUsd,
                    OldUsdFoil = result.OldUsdFoil,
                    OldUsdEtched = result.OldUsdEtched,
                    NewUsd = result.NewUsd,
                    NewUsdFoil = result.NewUsdFoil,
                    NewUsdEtched = result.NewUsdEtched
                });
            }

            double ru = result.RuConsumed;
            double currentTotal;
            double newTotal;
            do
            {
                currentTotal = totalRu;
                newTotal = currentTotal + ru;
            }
            while (Interlocked.CompareExchange(ref totalRu, newTotal, currentTotal) != currentTotal);
        }

        _logger.LogInformation("Processing CardItems container...");
        await ProcessContainerAsync(_cardItemsUpdater).ConfigureAwait(false);

        _logger.LogInformation("Processing SetCards container...");
        await ProcessContainerAsync(_setCardsUpdater).ConfigureAwait(false);

        _logger.LogInformation("Processing ArtistCards container...");
        await ProcessContainerAsync(_artistCardsUpdater).ConfigureAwait(false);

        _logger.LogInformation("Processing CardsByName container...");
        await ProcessContainerAsync(_cardsByNameUpdater).ConfigureAwait(false);

        _progressTracker.Complete();

        return new PriceUpdateResult
        {
            TotalCards = priceList.Count,
            UpdatedCards = updatedCount,
            SkippedCards = skippedCount,
            Errors = errorCount,
            TotalRuConsumed = totalRu
        };
    }
}
