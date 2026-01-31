using System;
using System.Threading.Tasks;
using Cli.MtgDiscovery.PriceUpdate.Cosmos.Containers;
using Cli.MtgDiscovery.PriceUpdate.ManaPool.Entities;
using Cli.MtgDiscovery.PriceUpdate.Mapping;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Extensions.Logging;

namespace Cli.MtgDiscovery.PriceUpdate.Updaters;

internal sealed class CardItemsPriceUpdater : IPriceUpdater
{
    private readonly ILogger<CardItemsPriceUpdater> _logger;
    private readonly IManaPoolToPricesMapper _priceMapper;
    private readonly CardItemsCosmosContainer _container;

    public string ContainerName => "CardItems";

    public CardItemsPriceUpdater(
        ILogger<CardItemsPriceUpdater> logger,
        IManaPoolToPricesMapper priceMapper)
    {
        _logger = logger;
        _priceMapper = priceMapper;
        _container = new CardItemsCosmosContainer(logger);
    }

    public async Task<PriceUpdateItemResult> UpdatePriceAsync(string scryfallId, ManaPoolPriceItem priceItem)
    {
        double totalRu = 0;

        try
        {
            ReadPointItem readItem = new()
            {
                Id = new ProvidedCosmosItemId(scryfallId),
                Partition = new ProvidedPartitionKeyValue(scryfallId)
            };

            OpResponse<ScryfallCardItemExtEntity> readResponse = await _container.ReadAsync<ScryfallCardItemExtEntity>(readItem).ConfigureAwait(false);

            if (readResponse.IsNotSuccessful())
            {
                return new PriceUpdateItemResult
                {
                    ScryfallId = scryfallId,
                    Container = ContainerName,
                    HasError = true,
                    ErrorMessage = $"Failed to read card: {readResponse.StatusCode}"
                };
            }

            ScryfallCardItemExtEntity entity = readResponse.Value;
            string cardName = entity.Data.name?.ToString() ?? "Unknown";
            string setCode = entity.Data.set?.ToString() ?? "Unknown";

            string oldUsd = entity.Data.prices?.usd?.ToString() ?? string.Empty;
            string oldUsdFoil = entity.Data.prices?.usd_foil?.ToString() ?? string.Empty;
            string oldUsdEtched = entity.Data.prices?.usd_etched?.ToString() ?? string.Empty;

            dynamic newPrices = _priceMapper.MapToPrices(priceItem);
            string newUsd = newPrices.usd?.ToString() ?? string.Empty;
            string newUsdFoil = newPrices.usd_foil?.ToString() ?? string.Empty;
            string newUsdEtched = newPrices.usd_etched?.ToString() ?? string.Empty;

            if (oldUsd == newUsd && oldUsdFoil == newUsdFoil && oldUsdEtched == newUsdEtched)
            {
                return new PriceUpdateItemResult
                {
                    ScryfallId = scryfallId,
                    Container = ContainerName,
                    CardName = cardName,
                    SetCode = setCode,
                    WasSkipped = true,
                    RuConsumed = totalRu,
                    OldUsd = oldUsd,
                    OldUsdFoil = oldUsdFoil,
                    OldUsdEtched = oldUsdEtched,
                    NewUsd = newUsd,
                    NewUsdFoil = newUsdFoil,
                    NewUsdEtched = newUsdEtched
                };
            }

            entity.Data.prices = newPrices;

            OpResponse<ScryfallCardItemExtEntity> upsertResponse = await _container.UpsertAsync(entity).ConfigureAwait(false);

            if (upsertResponse.IsNotSuccessful())
            {
                return new PriceUpdateItemResult
                {
                    ScryfallId = scryfallId,
                    Container = ContainerName,
                    CardName = cardName,
                    SetCode = setCode,
                    HasError = true,
                    ErrorMessage = $"Failed to upsert card: {upsertResponse.StatusCode}",
                    RuConsumed = totalRu
                };
            }

            return new PriceUpdateItemResult
            {
                ScryfallId = scryfallId,
                Container = ContainerName,
                CardName = cardName,
                SetCode = setCode,
                WasUpdated = true,
                RuConsumed = totalRu,
                OldUsd = oldUsd,
                OldUsdFoil = oldUsdFoil,
                OldUsdEtched = oldUsdEtched,
                NewUsd = newUsd,
                NewUsdFoil = newUsdFoil,
                NewUsdEtched = newUsdEtched
            };
        }
#pragma warning disable CA1031 // Catch general exception to log errors and continue processing
        catch (Exception ex)
#pragma warning restore CA1031
        {
            _logger.LogError(ex, "Error updating price for {ScryfallId} in {Container}", scryfallId, ContainerName);

            return new PriceUpdateItemResult
            {
                ScryfallId = scryfallId,
                Container = ContainerName,
                HasError = true,
                ErrorMessage = ex.Message,
                RuConsumed = totalRu
            };
        }
    }
}
