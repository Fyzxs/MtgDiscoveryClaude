using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Artists.Apis;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Aggregator.Artists.Apis;
using Lib.Aggregator.Artists.Entities;
using Lib.Aggregator.Artists.Mappers;
using Lib.Aggregator.Scryfall.Shared.Entities;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Artists.Queries;

internal sealed class QueryArtistAggregatorService : IArtistAggregatorService
{
    private readonly IArtistAdapterService _artistAdapterService;
    private readonly IArtistNameTrigramDataExtToItrEntityMapper _artistSearchMapper;
    private readonly IArtistCardExtToItrEntityMapper _artistCardMapper;

    public QueryArtistAggregatorService(ILogger logger) : this(
        new ArtistAdapterService(logger),
        new ArtistNameTrigramDataExtToItrEntityMapper(),
        new ArtistCardExtToItrEntityMapper())
    { }

    private QueryArtistAggregatorService(
        IArtistAdapterService artistAdapterService,
        IArtistNameTrigramDataExtToItrEntityMapper artistSearchMapper,
        IArtistCardExtToItrEntityMapper artistCardMapper)
    {
        _artistAdapterService = artistAdapterService;
        _artistSearchMapper = artistSearchMapper;
        _artistCardMapper = artistCardMapper;
    }

    public async Task<IOperationResponse<IArtistSearchResultCollectionItrEntity>> ArtistSearchAsync(IArtistSearchTermItrEntity searchTerm)
    {
        IOperationResponse<IEnumerable<ArtistNameTrigramDataExtEntity>> adapterResponse = await _artistAdapterService.SearchArtistsAsync(searchTerm).ConfigureAwait(false);

        if (adapterResponse.IsSuccess is false)
        {
            return new FailureOperationResponse<IArtistSearchResultCollectionItrEntity>(adapterResponse.OuterException);
        }

        // Map ExtEntity to ItrEntity
        List<IArtistSearchResultItrEntity> mappedArtists = [];
        foreach (ArtistNameTrigramDataExtEntity extEntity in adapterResponse.ResponseData)
        {
            IArtistSearchResultItrEntity itrEntity = await _artistSearchMapper.Map(extEntity).ConfigureAwait(false);
            mappedArtists.Add(itrEntity);
        }

        IArtistSearchResultCollectionItrEntity collection = new ArtistSearchResultCollectionItrEntity
        {
            Artists = mappedArtists
        };

        return new SuccessOperationResponse<IArtistSearchResultCollectionItrEntity>(collection);
    }

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByArtistAsync(IArtistIdItrEntity artistId)
    {
        IOperationResponse<IEnumerable<ScryfallArtistCardExtEntity>> adapterResponse = await _artistAdapterService.GetCardsByArtistIdAsync(artistId).ConfigureAwait(false);

        if (adapterResponse.IsSuccess is false)
        {
            return new FailureOperationResponse<ICardItemCollectionItrEntity>(adapterResponse.OuterException);
        }

        // Map ExtEntity to ItrEntity
        List<ICardItemItrEntity> mappedCards = [];
        foreach (ScryfallArtistCardExtEntity extEntity in adapterResponse.ResponseData)
        {
            ICardItemItrEntity itrEntity = await _artistCardMapper.Map(extEntity).ConfigureAwait(false);
            mappedCards.Add(itrEntity);
        }

        ICardItemCollectionItrEntity collection = new CardItemCollectionItrEntity
        {
            Data = mappedCards
        };

        return new SuccessOperationResponse<ICardItemCollectionItrEntity>(collection);
    }

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByArtistNameAsync(IArtistNameItrEntity artistName)
    {
        IOperationResponse<IEnumerable<ScryfallArtistCardExtEntity>> adapterResponse = await _artistAdapterService.GetCardsByArtistNameAsync(artistName).ConfigureAwait(false);

        if (adapterResponse.IsSuccess is false)
        {
            return new FailureOperationResponse<ICardItemCollectionItrEntity>(adapterResponse.OuterException);
        }

        // Map ExtEntity to ItrEntity
        List<ICardItemItrEntity> mappedCards = [];
        foreach (ScryfallArtistCardExtEntity extEntity in adapterResponse.ResponseData)
        {
            ICardItemItrEntity itrEntity = await _artistCardMapper.Map(extEntity).ConfigureAwait(false);
            mappedCards.Add(itrEntity);
        }

        ICardItemCollectionItrEntity collection = new CardItemCollectionItrEntity
        {
            Data = mappedCards
        };

        return new SuccessOperationResponse<ICardItemCollectionItrEntity>(collection);
    }
}
