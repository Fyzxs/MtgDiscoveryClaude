using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Artists.Apis;
using Lib.Adapter.Artists.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Aggregator.Artists.Apis;
using Lib.Aggregator.Artists.Queries.Entities;
using Lib.Aggregator.Artists.Queries.Mappers;
using Lib.Aggregator.Scryfall.Shared.Entities;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Artists.Queries;

internal sealed class QueryArtistAggregatorService : IArtistAggregatorService
{
    private readonly IArtistAdapterService _artistAdapterService;
    private readonly IArtistNameTrigramDataExtToItrEntityMapper _artistSearchToItrMapper;
    private readonly IArtistCardExtToItrEntityMapper _artistCardToItrMapper;
    private readonly IArtistSearchTermItrToXfrMapper _artistSearchItrToXfrMapper;
    private readonly IArtistIdItrToXfrMapper _aristIdToXfrMapper;
    private readonly IArtistNameItrToXfrMapper _artistNameToXfrMapper;
    private readonly IArtistSearchExtToItrMapper _artistSearchResultCollectionMapper;

    public QueryArtistAggregatorService(ILogger logger) : this(
        new ArtistAdapterService(logger),
        new ArtistNameTrigramDataExtToItrEntityMapper(),
        new ArtistCardExtToItrEntityMapper(),
        new ArtistSearchTermItrToXfrMapper(),
        new ArtistIdItrToXfrMapper(),
        new ArtistNameItrToXfrMapper(),
        new ArtistSearchExtToItrMapper())
    { }

    private QueryArtistAggregatorService(IArtistAdapterService artistAdapterService,
        IArtistNameTrigramDataExtToItrEntityMapper artistSearchToItrMapper,
        IArtistCardExtToItrEntityMapper artistCardToItrMapper,
        IArtistSearchTermItrToXfrMapper artistSearchItrToXfrMapper,
        IArtistIdItrToXfrMapper aristIdToXfrMapper,
        IArtistNameItrToXfrMapper artistNameToXfrMapper,
        IArtistSearchExtToItrMapper artistSearchResultCollectionMapper)
    {
        _artistAdapterService = artistAdapterService;
        _artistSearchToItrMapper = artistSearchToItrMapper;
        _artistCardToItrMapper = artistCardToItrMapper;
        _artistSearchItrToXfrMapper = artistSearchItrToXfrMapper;
        _aristIdToXfrMapper = aristIdToXfrMapper;
        _artistNameToXfrMapper = artistNameToXfrMapper;
        _artistSearchResultCollectionMapper = artistSearchResultCollectionMapper;
    }

    public async Task<IOperationResponse<IArtistSearchResultCollectionItrEntity>> ArtistSearchAsync(IArtistSearchTermItrEntity searchTerm)
    {
        IArtistSearchTermXfrEntity mappedEntity = await _artistSearchItrToXfrMapper.Map(searchTerm).ConfigureAwait(false);
        IOperationResponse<IEnumerable<ArtistNameTrigramDataExtEntity>> adapterResponse = await _artistAdapterService.SearchArtistsAsync(mappedEntity).ConfigureAwait(false);

        if (adapterResponse.IsFailure)
        {
            return new FailureOperationResponse<IArtistSearchResultCollectionItrEntity>(adapterResponse.OuterException);
        }

        IArtistSearchResultCollectionItrEntity collection = await _artistSearchResultCollectionMapper.Map(adapterResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<IArtistSearchResultCollectionItrEntity>(collection);
    }

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByArtistAsync(IArtistIdItrEntity artistId)
    {
        IArtistIdXfrEntity xfrEntity = await _aristIdToXfrMapper.Map(artistId).ConfigureAwait(false);
        IOperationResponse<IEnumerable<ScryfallArtistCardExtEntity>> adapterResponse = await _artistAdapterService.CardsByArtistIdAsync(xfrEntity).ConfigureAwait(false);

        if (adapterResponse.IsFailure)
        {
            return new FailureOperationResponse<ICardItemCollectionItrEntity>(adapterResponse.OuterException);
        }

        // Map ExtEntity to ItrEntity
        List<ICardItemItrEntity> mappedCards = [];
        foreach (ScryfallArtistCardExtEntity extEntity in adapterResponse.ResponseData)
        {
            ICardItemItrEntity itrEntity = await _artistCardToItrMapper.Map(extEntity).ConfigureAwait(false);
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
        IArtistNameXfrEntity xfrEntity = await _artistNameToXfrMapper.Map(artistName).ConfigureAwait(false);
        IOperationResponse<IEnumerable<ScryfallArtistCardExtEntity>> adapterResponse = await _artistAdapterService.CardsByArtistNameAsync(xfrEntity).ConfigureAwait(false);

        if (adapterResponse.IsFailure)
        {
            return new FailureOperationResponse<ICardItemCollectionItrEntity>(adapterResponse.OuterException);
        }

        // Map ExtEntity to ItrEntity
        List<ICardItemItrEntity> mappedCards = [];
        foreach (ScryfallArtistCardExtEntity extEntity in adapterResponse.ResponseData)
        {
            ICardItemItrEntity itrEntity = await _artistCardToItrMapper.Map(extEntity).ConfigureAwait(false);
            mappedCards.Add(itrEntity);
        }

        ICardItemCollectionItrEntity collection = new CardItemCollectionItrEntity
        {
            Data = mappedCards
        };

        return new SuccessOperationResponse<ICardItemCollectionItrEntity>(collection);
    }
}
