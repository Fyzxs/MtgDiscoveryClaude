using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Artists.Apis;
using Lib.Adapter.Artists.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.ArtistCards;
using Lib.Aggregator.Artists.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.Artists;
using Lib.Shared.DataModels.Entities.Itrs.Cards;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Artists.Queries;

internal sealed class CardsByArtistNameAggregatorService : ICardsByArtistNameAggregatorService
{
    private readonly IArtistAdapterService _artistAdapterService;
    private readonly IArtistNameItrToXfrMapper _artistNameToXfrMapper;
    private readonly ICollectionArtistCardExtToOufMapper _artistCardCollectionMapper;
    private readonly ICollectionCardItemItrToOufMapper _cardItemItrToOufMapper;

    public CardsByArtistNameAggregatorService(ILogger logger) : this(
        new ArtistAdapterService(logger),
        new ArtistNameItrToXfrMapper(),
        new CollectionArtistCardExtToOufMapper(),
        new CollectionCardItemItrToOufMapper())
    { }

    private CardsByArtistNameAggregatorService(
        IArtistAdapterService artistAdapterService,
        IArtistNameItrToXfrMapper artistNameToXfrMapper,
        ICollectionArtistCardExtToOufMapper artistCardCollectionMapper,
        ICollectionCardItemItrToOufMapper cardItemItrToOufMapper)
    {
        _artistAdapterService = artistAdapterService;
        _artistNameToXfrMapper = artistNameToXfrMapper;
        _artistCardCollectionMapper = artistCardCollectionMapper;
        _cardItemItrToOufMapper = cardItemItrToOufMapper;
    }

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> Execute(
        IArtistNameItrEntity input,
        CancellationToken cancellationToken)
    {
        IArtistNameXfrEntity xfrEntity = await _artistNameToXfrMapper.Map(input).ConfigureAwait(false);
        IOperationResponse<IEnumerable<ScryfallArtistCardExtEntity>> adapterResponse = await _artistAdapterService.CardsByArtistNameAsync(xfrEntity, cancellationToken).ConfigureAwait(false);

        if (adapterResponse.IsFailure)
        {
            return new FailureOperationResponse<ICardItemCollectionOufEntity>(adapterResponse.OuterException);
        }

        IEnumerable<ICardItemItrEntity> mappedCards = await _artistCardCollectionMapper.Map(adapterResponse.ResponseData).ConfigureAwait(false);
        ICardItemCollectionOufEntity collection = await _cardItemItrToOufMapper.Map(mappedCards).ConfigureAwait(false);

        return new SuccessOperationResponse<ICardItemCollectionOufEntity>(collection);
    }
}
