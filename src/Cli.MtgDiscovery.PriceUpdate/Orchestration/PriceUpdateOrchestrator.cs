using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cli.MtgDiscovery.PriceUpdate.Configuration;
using Cli.MtgDiscovery.PriceUpdate.Dashboard;
using Cli.MtgDiscovery.PriceUpdate.ErrorTracking;
using Cli.MtgDiscovery.PriceUpdate.ManaPool;
using Cli.MtgDiscovery.PriceUpdate.ManaPool.Entities;
using Cli.MtgDiscovery.PriceUpdate.PriceChangeLogging;
using Cli.MtgDiscovery.PriceUpdate.Updaters;

namespace Cli.MtgDiscovery.PriceUpdate.Orchestration;

internal sealed class PriceUpdateOrchestrator : IPriceUpdateOrchestrator
{
    private readonly PriceUpdateConfiguration _config;
    private readonly IManaPoolApiClient _apiClient;
    private readonly IPriceUpdater _cardItemsUpdater;
    private readonly IPriceUpdater _setCardsUpdater;
    private readonly IPriceUpdater _artistCardsUpdater;
    private readonly IPriceUpdater _cardsByNameUpdater;
    private readonly IPriceUpdateErrorLogger _errorLogger;
    private readonly IPriceChangeLogger _priceChangeLogger;
    private readonly IPriceUpdateDashboard _dashboard;

    public PriceUpdateOrchestrator(
        PriceUpdateConfiguration config,
        IManaPoolApiClient apiClient,
        IPriceUpdater cardItemsUpdater,
        IPriceUpdater setCardsUpdater,
        IPriceUpdater artistCardsUpdater,
        IPriceUpdater cardsByNameUpdater,
        IPriceUpdateErrorLogger errorLogger,
        IPriceChangeLogger priceChangeLogger,
        IPriceUpdateDashboard dashboard)
    {
        _config = config;
        _apiClient = apiClient;
        _cardItemsUpdater = cardItemsUpdater;
        _setCardsUpdater = setCardsUpdater;
        _artistCardsUpdater = artistCardsUpdater;
        _cardsByNameUpdater = cardsByNameUpdater;
        _errorLogger = errorLogger;
        _priceChangeLogger = priceChangeLogger;
        _dashboard = dashboard;
    }

    public async Task<PriceUpdateResult> ExecuteAsync()
    {
        _dashboard.StartTimer();
        _dashboard.SetStatus("Fetching prices from ManaPool API...");
        _dashboard.AddLog("Starting price update process");

        IReadOnlyDictionary<string, ManaPoolPriceItem> prices = await _apiClient.FetchAllPricesAsync().ConfigureAwait(false);

        IReadOnlyList<KeyValuePair<string, ManaPoolPriceItem>> priceList = _config.TestMode
            ? prices.Take(_config.TestCardLimit).ToList()
            : [.. prices];

        _dashboard.AddLog($"Fetched {prices.Count:N0} prices from ManaPool");

        int updatedCount = 0;
        int skippedCount = 0;
        int errorCount = 0;
        double totalRu = 0;

        List<(IPriceUpdater Updater, int Index)> containers =
        [
            (_cardItemsUpdater, 1),
            (_setCardsUpdater, 2),
            (_artistCardsUpdater, 3),
            (_cardsByNameUpdater, 4)
        ];

        foreach ((IPriceUpdater updater, int containerIndex) in containers)
        {
            if (_dashboard.GetCancellationToken().IsCancellationRequested)
            {
                _dashboard.AddLog("Operation cancelled by user");
                break;
            }

            _dashboard.SetStatus($"Processing {updater.ContainerName}...");
            _dashboard.SetContainer(updater.ContainerName, containerIndex, priceList.Count);
            _dashboard.AddLog($"Starting {updater.ContainerName} container");

            int itemIndex = 0;
            foreach (KeyValuePair<string, ManaPoolPriceItem> priceEntry in priceList)
            {
                if (_dashboard.GetCancellationToken().IsCancellationRequested)
                {
                    break;
                }

                itemIndex++;
                _dashboard.UpdateProgress(itemIndex, priceEntry.Key);

                PriceUpdateItemResult result = await UpdateWithRetryAsync(updater, priceEntry.Key, priceEntry.Value)
                    .ConfigureAwait(false);

                ProcessResult(result, ref updatedCount, ref skippedCount, ref errorCount, ref totalRu);

                if (_config.DelayBetweenOperationsMs > 0)
                {
                    await Task.Delay(_config.DelayBetweenOperationsMs).ConfigureAwait(false);
                }
            }

            _dashboard.AddLog($"Completed {updater.ContainerName}: {itemIndex:N0} items processed");
        }

        string completionMessage = $"Completed! Updated: {updatedCount:N0}, Skipped: {skippedCount:N0}, Errors: {errorCount:N0}, RU: {totalRu:N2}";
        _dashboard.MarkComplete(completionMessage);

        return new PriceUpdateResult
        {
            TotalCards = priceList.Count,
            UpdatedCards = updatedCount,
            SkippedCards = skippedCount,
            Errors = errorCount,
            TotalRuConsumed = totalRu
        };
    }

    private async Task<PriceUpdateItemResult> UpdateWithRetryAsync(IPriceUpdater updater, string scryfallId, ManaPoolPriceItem priceItem)
    {
        for (int attempt = 1; attempt <= _config.CosmosRetryCount; attempt++)
        {
            PriceUpdateItemResult result = await updater
                .UpdatePriceAsync(scryfallId, priceItem)
                .ConfigureAwait(false);

            if (result.HasError && result.ErrorMessage?.Contains("429", System.StringComparison.Ordinal) == true)
            {
                if (attempt < _config.CosmosRetryCount)
                {
                    int delay = _config.CosmosRetryDelayMs * attempt;
                    _dashboard.AddLog($"429 retry {attempt}/{_config.CosmosRetryCount} for {scryfallId}");

                    await Task.Delay(delay).ConfigureAwait(false);
                    continue;
                }
            }

            return result;
        }

        return new PriceUpdateItemResult
        {
            ScryfallId = scryfallId,
            Container = updater.ContainerName,
            HasError = true,
            ErrorMessage = $"Failed after {_config.CosmosRetryCount} retry attempts"
        };
    }

    private void ProcessResult(PriceUpdateItemResult result, ref int updatedCount, ref int skippedCount, ref int errorCount, ref double totalRu)
    {
        if (result.HasError)
        {
            errorCount++;
            _dashboard.IncrementError();
            _errorLogger.LogError(new PriceUpdateError
            {
                ScryfallId = result.ScryfallId,
                Container = result.Container,
                ErrorMessage = result.ErrorMessage
            });
        }
        else if (result.WasSkipped)
        {
            skippedCount++;
            _dashboard.IncrementSkipped();
        }
        else if (result.WasUpdated)
        {
            updatedCount++;
            _dashboard.IncrementUpdated();
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

        totalRu += result.RuConsumed;
        _dashboard.AddRu(result.RuConsumed);
    }
}
