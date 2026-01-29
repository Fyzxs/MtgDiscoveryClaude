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

internal sealed class ArtistCardsPriceUpdater : IPriceUpdater
{
    private readonly ILogger<ArtistCardsPriceUpdater> _logger;
    private readonly IManaPoolToPricesMapper _priceMapper;
    private readonly ArtistCardsCosmosContainer _container;

    public string ContainerName => "ArtistCards";

    public ArtistCardsPriceUpdater(
        ILogger<ArtistCardsPriceUpdater> logger,
        IManaPoolToPricesMapper priceMapper)
    {
        _logger = logger;
        _priceMapper = priceMapper;
        _container = new ArtistCardsCosmosContainer(logger);
    }

    public async Task<PriceUpdateItemResult> UpdatePriceAsync(string scryfallId, ManaPoolPriceItem priceItem)
    {
        double totalRu = 0;

        try
        {
            QueryDefinition query = new QueryDefinition("SELECT * FROM c WHERE c.Data.id = @scryfallId")
                .WithParameter("@scryfallId", scryfallId);

            OpResponse<IEnumerable<ScryfallArtistCardExtEntity>> queryResponse = await _container
                .QueryAsync<ScryfallArtistCardExtEntity>(query, PartitionKey.None)
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

            List<ScryfallArtistCardExtEntity> entities = [.. queryResponse.Value];

            if (entities.Count == 0)
            {
                return new PriceUpdateItemResult
                {
                    ScryfallId = scryfallId,
                    Container = ContainerName,
                    WasSkipped = true,
                    RuConsumed = totalRu
                };
            }

            ScryfallArtistCardExtEntity firstEntity = entities[0];
            string cardName = firstEntity.Data.name?.ToString() ?? "Unknown";
            string setCode = firstEntity.Data.set?.ToString() ?? "Unknown";

            string oldUsd = firstEntity.Data.prices?.usd?.ToString() ?? string.Empty;
            string oldUsdFoil = firstEntity.Data.prices?.usd_foil?.ToString() ?? string.Empty;
            string oldUsdEtched = firstEntity.Data.prices?.usd_etched?.ToString() ?? string.Empty;

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

            foreach (ScryfallArtistCardExtEntity entity in entities)
            {
                entity.Data.prices = newPrices;

                OpResponse<ScryfallArtistCardExtEntity> upsertResponse = await _container.UpsertAsync(entity).ConfigureAwait(false);

                if (upsertResponse.IsNotSuccessful())
                {
                    _logger.LogWarning(
                        "Failed to upsert artist card for {ScryfallId} with artist_id {ArtistId}: {StatusCode}",
                        scryfallId,
                        entity.ArtistId,
                        upsertResponse.StatusCode);
                }
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
