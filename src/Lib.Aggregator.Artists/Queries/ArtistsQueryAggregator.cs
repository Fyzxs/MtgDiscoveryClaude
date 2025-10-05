using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Artists.Apis;
using Lib.Adapter.Artists.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Aggregator.Artists.Apis;
using Lib.Aggregator.Artists.Queries.Mappers;
using Lib.Aggregator.Scryfall.Shared.Entities;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Artists.Queries;

internal sealed class ArtistsQueryAggregator : IArtistAggregatorService
{
    private readonly IArtistAdapterService _artistAdapterService;
    private readonly IArtistCardExtToItrEntityMapper _artistCardToItrMapper;
    private readonly IArtistSearchTermItrToXfrMapper _artistSearchItrToXfrMapper;
    private readonly IArtistIdItrToXfrMapper _aristIdToXfrMapper;
    private readonly IArtistNameItrToXfrMapper _artistNameToXfrMapper;
    private readonly IArtistSearchExtToItrMapper _artistSearchResultCollectionMapper;

    public ArtistsQueryAggregator(ILogger logger) : this(
        new ArtistAdapterService(logger),
        new ArtistCardExtToItrEntityMapper(),
        new ArtistSearchTermItrToXfrMapper(),
        new ArtistIdItrToXfrMapper(),
        new ArtistNameItrToXfrMapper(),
        new ArtistSearchExtToItrMapper())
    { }

    private ArtistsQueryAggregator(IArtistAdapterService artistAdapterService,
        IArtistCardExtToItrEntityMapper artistCardToItrMapper,
        IArtistSearchTermItrToXfrMapper artistSearchItrToXfrMapper,
        IArtistIdItrToXfrMapper aristIdToXfrMapper,
        IArtistNameItrToXfrMapper artistNameToXfrMapper,
        IArtistSearchExtToItrMapper artistSearchResultCollectionMapper)
    {
        _artistAdapterService = artistAdapterService;
        _artistCardToItrMapper = artistCardToItrMapper;
        _artistSearchItrToXfrMapper = artistSearchItrToXfrMapper;
        _aristIdToXfrMapper = aristIdToXfrMapper;
        _artistNameToXfrMapper = artistNameToXfrMapper;
        _artistSearchResultCollectionMapper = artistSearchResultCollectionMapper;
    }

    public async Task<IOperationResponse<IArtistSearchResultCollectionOufEntity>> ArtistSearchAsync(IArtistSearchTermItrEntity searchTerm)
    {
        IArtistSearchTermXfrEntity mappedEntity = await _artistSearchItrToXfrMapper.Map(searchTerm).ConfigureAwait(false);
        IOperationResponse<IEnumerable<ArtistNameTrigramDataExtEntity>> adapterResponse = await _artistAdapterService.SearchArtistsAsync(mappedEntity).ConfigureAwait(false);

        if (adapterResponse.IsFailure)
        {
            return new FailureOperationResponse<IArtistSearchResultCollectionOufEntity>(adapterResponse.OuterException);
        }

        IArtistSearchResultCollectionOufEntity collection = await _artistSearchResultCollectionMapper.Map(adapterResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<IArtistSearchResultCollectionOufEntity>(collection);
    }

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByArtistAsync(IArtistIdItrEntity artistId)
    {
        IArtistIdXfrEntity xfrEntity = await _aristIdToXfrMapper.Map(artistId).ConfigureAwait(false);
        IOperationResponse<IEnumerable<ScryfallArtistCardExtEntity>> adapterResponse = await _artistAdapterService.CardsByArtistIdAsync(xfrEntity).ConfigureAwait(false);

        if (adapterResponse.IsFailure)
        {
            return new FailureOperationResponse<ICardItemCollectionOufEntity>(adapterResponse.OuterException);
        }

        // Map ExtEntity to ItrEntity
        List<ICardItemItrEntity> mappedCards = [];
        foreach (ScryfallArtistCardExtEntity extEntity in adapterResponse.ResponseData)
        {
            ICardItemItrEntity itrEntity = await _artistCardToItrMapper.Map(extEntity).ConfigureAwait(false);
            mappedCards.Add(itrEntity);
        }

        ICardItemCollectionOufEntity collection = new CardItemCollectionOufEntity
        {
            Data = mappedCards
        };

        return new SuccessOperationResponse<ICardItemCollectionOufEntity>(collection);
    }

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByArtistNameAsync(IArtistNameItrEntity artistName)
    {
        IArtistNameXfrEntity xfrEntity = await _artistNameToXfrMapper.Map(artistName).ConfigureAwait(false);
        IOperationResponse<IEnumerable<ScryfallArtistCardExtEntity>> adapterResponse = await _artistAdapterService.CardsByArtistNameAsync(xfrEntity).ConfigureAwait(false);

        if (adapterResponse.IsFailure)
        {
            return new FailureOperationResponse<ICardItemCollectionOufEntity>(adapterResponse.OuterException);
        }

        // Map ExtEntity to ItrEntity
        List<ICardItemItrEntity> mappedCards = [];
        foreach (ScryfallArtistCardExtEntity extEntity in adapterResponse.ResponseData)
        {
            ICardItemItrEntity itrEntity = await _artistCardToItrMapper.Map(extEntity).ConfigureAwait(false);
            mappedCards.Add(itrEntity);
        }

        ICardItemCollectionOufEntity collection = new CardItemCollectionOufEntity
        {
            Data = mappedCards
        };

        return new SuccessOperationResponse<ICardItemCollectionOufEntity>(collection);
    }
}
