using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Artists.Apis;
using Lib.Adapter.Artists.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Aggregator.Artists.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Artists.Queries;

internal sealed class CardsByArtistAggregatorService : ICardsByArtistAggregatorService
{
    private readonly IArtistAdapterService _artistAdapterService;
    private readonly IArtistIdItrToXfrMapper _aristIdToXfrMapper;
    private readonly ICollectionArtistCardExtToItrMapper _artistCardCollectionMapper;
    private readonly ICollectionCardItemItrToOufMapper _cardItemItrToOufMapper;

    public CardsByArtistAggregatorService(ILogger logger) : this(
        new ArtistAdapterService(logger),
        new ArtistIdItrToXfrMapper(),
        new CollectionArtistCardExtToItrMapper(),
        new CollectionCardItemItrToOufMapper())
    { }

    private CardsByArtistAggregatorService(
        IArtistAdapterService artistAdapterService,
        IArtistIdItrToXfrMapper aristIdToXfrMapper,
        ICollectionArtistCardExtToItrMapper artistCardCollectionMapper,
        ICollectionCardItemItrToOufMapper cardItemItrToOufMapper)
    {
        _artistAdapterService = artistAdapterService;
        _aristIdToXfrMapper = aristIdToXfrMapper;
        _artistCardCollectionMapper = artistCardCollectionMapper;
        _cardItemItrToOufMapper = cardItemItrToOufMapper;
    }

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> Execute(IArtistIdItrEntity input)
    {
        IArtistIdXfrEntity xfrEntity = await _aristIdToXfrMapper.Map(input).ConfigureAwait(false);
        IOperationResponse<IEnumerable<ScryfallArtistCardExtEntity>> adapterResponse = await _artistAdapterService.CardsByArtistIdAsync(xfrEntity).ConfigureAwait(false);

        if (adapterResponse.IsFailure)
        {
            return new FailureOperationResponse<ICardItemCollectionOufEntity>(adapterResponse.OuterException);
        }

        IEnumerable<ICardItemItrEntity> mappedCards = await _artistCardCollectionMapper.Map(adapterResponse.ResponseData).ConfigureAwait(false);
        ICardItemCollectionOufEntity collection = await _cardItemItrToOufMapper.Map(mappedCards).ConfigureAwait(false);

        return new SuccessOperationResponse<ICardItemCollectionOufEntity>(collection);
    }
}
