using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Artists.Apis;
using Lib.Adapter.Artists.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Aggregator.Artists.Queries.Mappers;
using Lib.Aggregator.Scryfall.Shared.Entities;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Artists.Queries.CardsByArtistName;

internal sealed class CardsByArtistNameAggregatorService : ICardsByArtistNameAggregatorService
{
    private readonly IArtistAdapterService _artistAdapterService;
    private readonly IArtistNameItrToXfrMapper _artistNameToXfrMapper;
    private readonly ICollectionArtistCardExtToItrMapper _artistCardCollectionMapper;
    private readonly ICollectionCardItemItrToOufMapper _cardItemItrToOufMapper;

    public CardsByArtistNameAggregatorService(ILogger logger) : this(
        new ArtistAdapterService(logger),
        new ArtistNameItrToXfrMapper(),
        new CollectionArtistCardExtToItrMapper(),
        new CollectionCardItemItrToOufMapper())
    { }

    private CardsByArtistNameAggregatorService(
        IArtistAdapterService artistAdapterService,
        IArtistNameItrToXfrMapper artistNameToXfrMapper,
        ICollectionArtistCardExtToItrMapper artistCardCollectionMapper,
        ICollectionCardItemItrToOufMapper cardItemItrToOufMapper)
    {
        _artistAdapterService = artistAdapterService;
        _artistNameToXfrMapper = artistNameToXfrMapper;
        _artistCardCollectionMapper = artistCardCollectionMapper;
        _cardItemItrToOufMapper = cardItemItrToOufMapper;
    }

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByArtistNameAsync(IArtistNameItrEntity artistName)
    {
        IArtistNameXfrEntity xfrEntity = await _artistNameToXfrMapper.Map(artistName).ConfigureAwait(false);
        IOperationResponse<IEnumerable<ScryfallArtistCardExtEntity>> adapterResponse = await _artistAdapterService.CardsByArtistNameAsync(xfrEntity).ConfigureAwait(false);

        if (adapterResponse.IsFailure)
        {
            return new FailureOperationResponse<ICardItemCollectionOufEntity>(adapterResponse.OuterException);
        }

        IEnumerable<ICardItemItrEntity> mappedCards = await _artistCardCollectionMapper.Map(adapterResponse.ResponseData).ConfigureAwait(false);
        ICardItemCollectionOufEntity collection = await _cardItemItrToOufMapper.Map(mappedCards).ConfigureAwait(false);

        return new SuccessOperationResponse<ICardItemCollectionOufEntity>(collection);
    }
}
