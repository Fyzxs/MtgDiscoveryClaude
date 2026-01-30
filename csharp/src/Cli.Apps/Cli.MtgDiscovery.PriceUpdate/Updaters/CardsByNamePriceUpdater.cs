#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cli.MtgDiscovery.PriceUpdate.Cosmos.Containers;
using Cli.MtgDiscovery.PriceUpdate.ManaPool.Entities;
using Cli.MtgDiscovery.PriceUpdate.Mapping;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace Cli.MtgDiscovery.PriceUpdate.Updaters;

internal sealed class CardsByNamePriceUpdater : IPriceUpdater
{
    private readonly ILogger<CardsByNamePriceUpdater> _logger;
    private readonly IManaPoolToPricesMapper _priceMapper;
    private readonly CardsByNameCosmosContainer _container;

    public string ContainerName => "CardsByName";

    public CardsByNamePriceUpdater(
        ILogger<CardsByNamePriceUpdater> logger,
        IManaPoolToPricesMapper priceMapper)
    {
        _logger = logger;
        _priceMapper = priceMapper;
        _container = new CardsByNameCosmosContainer(logger);
    }

    public async Task<PriceUpdateItemResult> UpdatePriceAsync(string scryfallId, ManaPoolPriceItem priceItem)
    {
        double totalRu = 0;

        try
        {
            QueryDefinition query = new QueryDefinition("SELECT * FROM c WHERE c.Data.id = @scryfallId")
                .WithParameter("@scryfallId", scryfallId);

            OpResponse<IEnumerable<ScryfallCardByNameExtEntity>> queryResponse = await _container
                .QueryAsync<ScryfallCardByNameExtEntity>(query, PartitionKey.None)
                .ConfigureAwait(false);

            if (queryResponse.IsNotSuccessful())
            {
                return new PriceUpdateItemResult
                {
                    ScryfallId = scryfallId,
                    Container = ContainerName,
                    HasError = true,
                    ErrorMessage = $"Failed to query card: {queryResponse.StatusCode}"
                };
            }

            IEnumerable<ScryfallCardByNameExtEntity> entities = queryResponse.Value;
            ScryfallCardByNameExtEntity? entity = entities.FirstOrDefault();

            if (entity is null)
            {
                return new PriceUpdateItemResult
                {
                    ScryfallId = scryfallId,
                    Container = ContainerName,
                    WasSkipped = true,
                    RuConsumed = totalRu
                };
            }

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

            OpResponse<ScryfallCardByNameExtEntity> upsertResponse = await _container.UpsertAsync(entity).ConfigureAwait(false);

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
