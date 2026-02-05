using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Cli.Sealed.Ingestion.Dtos;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Cosmos.Apis;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Extensions.Logging;

namespace Cli.Sealed.Ingestion.Ingestion;

internal sealed class SealedProductIngestionService : ISealedProductIngestionService
{
    private readonly ILogger _logger;
    private readonly ScryfallSetCodeIndexGopher _setCodeIndexGopher;
    private readonly SealedProductsScribe _sealedProductsScribe;
    private readonly ConcurrentDictionary<string, string> _setIdCache = new(StringComparer.OrdinalIgnoreCase);

    public SealedProductIngestionService(ILogger logger)
    {
        _logger = logger;
        _setCodeIndexGopher = new ScryfallSetCodeIndexGopher(logger);
        _sealedProductsScribe = new SealedProductsScribe(logger);
    }

    public async Task<bool> IngestProductAsync(
        string setCode,
        string setName,
        MtgJsonSealedProductDto product,
        CancellationToken cancellationToken)
    {
        try
        {
            string setId = await GetSetIdAsync(setCode, cancellationToken).ConfigureAwait(false);

            if (string.IsNullOrEmpty(setId))
            {
                _logger.LogWarning("Could not find setId for setCode {SetCode}, skipping product {ProductName}", setCode, product.Name);
                return false;
            }

            SealedProductExtEntity entity = CreateEntity(setCode, setName, setId, product);

            OpResponse<SealedProductExtEntity> response = await _sealedProductsScribe
                .UpsertAsync(entity, cancellationToken)
                .ConfigureAwait(false);

            if (response.IsSuccessful())
            {
                _logger.LogDebug("Upserted: {ProductName}", product.Name);
                return true;
            }

            _logger.LogWarning("Failed to upsert {ProductName}: {StatusCode}", product.Name, response.StatusCode);
            return false;
        }
#pragma warning disable CA1031 // Catch general exception in ingestion to continue with other products
        catch (Exception ex)
#pragma warning restore CA1031
        {
            _logger.LogError(ex, "Error ingesting product {ProductName}", product.Name);
            return false;
        }
    }

    private async Task<string> GetSetIdAsync(string setCode, CancellationToken cancellationToken)
    {
        if (_setIdCache.TryGetValue(setCode, out string cachedSetId))
        {
            return cachedSetId;
        }

#pragma warning disable CA1308 // SetCode in Cosmos is lowercase
        string lowercaseSetCode = setCode.ToLowerInvariant();
#pragma warning restore CA1308

        ReadPointItem readPoint = new()
        {
            Id = new ProvidedCosmosItemId(lowercaseSetCode),
            Partition = new ProvidedPartitionKeyValue(lowercaseSetCode)
        };

        OpResponse<ScryfallSetCodeIndexExtEntity> response = await _setCodeIndexGopher
            .ReadAsync<ScryfallSetCodeIndexExtEntity>(readPoint, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsSuccessful() && response.Value is not null)
        {
            string setId = response.Value.SetId;
            _ = _setIdCache.TryAdd(setCode, setId);
            return setId;
        }

        return null;
    }

    private static SealedProductExtEntity CreateEntity(
        string setCode,
        string setName,
        string setId,
        MtgJsonSealedProductDto product)
    {
#pragma warning disable CA1308 // SetCode in URL should be lowercase
        string imageUrl = $"https://img.mtgdiscovery.com/mtgsealed/{setCode.ToLowerInvariant()}/{product.Uuid}.jpg";
#pragma warning restore CA1308

        return new SealedProductExtEntity
        {
            Uuid = product.Uuid,
            SetId = setId,
            SetCode = setCode,
            SetName = setName,
            Name = product.Name,
            Category = product.Category,
            Subtype = product.Subtype,
            CardCount = product.CardCount,
            ReleaseDate = product.ReleaseDate,
            TcgplayerProductId = product.Identifiers?.TcgplayerProductId,
            McmId = product.Identifiers?.McmId,
            CardtraderId = product.Identifiers?.CardTraderId,
            ImageUrl = imageUrl,
            PurchaseUrlTcgplayer = product.PurchaseUrls?.Tcgplayer,
            PurchaseUrlCardmarket = product.PurchaseUrls?.Cardmarket,
            PurchaseUrlCardKingdom = product.PurchaseUrls?.CardKingdom
        };
    }
}
