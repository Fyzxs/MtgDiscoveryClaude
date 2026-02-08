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

internal sealed class CardsByArtistAggregatorService : ICardsByArtistAggregatorService
{
    private readonly IArtistAdapterService _artistAdapterService;
    private readonly IArtistIdItrToXfrMapper _artistIdToXfrMapper;
    private readonly ICollectionArtistCardExtToOufMapper _artistCardCollectionMapper;
    private readonly ICollectionCardItemItrToOufMapper _cardItemItrToOufMapper;

    public CardsByArtistAggregatorService(ILogger logger) : this(
        new ArtistAdapterService(logger),
        new ArtistIdItrToXfrMapper(),
        new CollectionArtistCardExtToOufMapper(),
        new CollectionCardItemItrToOufMapper())
    { }

    private CardsByArtistAggregatorService(
        IArtistAdapterService artistAdapterService,
        IArtistIdItrToXfrMapper artistIdToXfrMapper,
        ICollectionArtistCardExtToOufMapper artistCardCollectionMapper,
        ICollectionCardItemItrToOufMapper cardItemItrToOufMapper)
    {
        _artistAdapterService = artistAdapterService;
        _artistIdToXfrMapper = artistIdToXfrMapper;
        _artistCardCollectionMapper = artistCardCollectionMapper;
        _cardItemItrToOufMapper = cardItemItrToOufMapper;
    }

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> Execute(
        IArtistIdItrEntity input,
        CancellationToken cancellationToken)
    {
        IArtistIdXfrEntity xfrEntity = await _artistIdToXfrMapper.Map(input).ConfigureAwait(false);
        IOperationResponse<IEnumerable<ScryfallArtistCardExtEntity>> adapterResponse = await _artistAdapterService.CardsByArtistIdAsync(xfrEntity, cancellationToken).ConfigureAwait(false);

        if (adapterResponse.IsFailure)
        {
            return new FailureOperationResponse<ICardItemCollectionOufEntity>(adapterResponse.OuterException);
        }

        IEnumerable<ICardItemItrEntity> mappedCards = await _artistCardCollectionMapper.Map(adapterResponse.ResponseData).ConfigureAwait(false);
        ICardItemCollectionOufEntity collection = await _cardItemItrToOufMapper.Map(mappedCards).ConfigureAwait(false);

        return new SuccessOperationResponse<ICardItemCollectionOufEntity>(collection);
    }
}
