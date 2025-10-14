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

namespace Lib.Aggregator.Artists.Queries.CardsByArtist;

internal sealed class CardsByArtistAggregatorService : ICardsByArtistAggregatorService
{
    private readonly IArtistAdapterService _artistAdapterService;
    private readonly IArtistIdItrToXfrMapper _aristIdToXfrMapper;
    private readonly IArtistCardExtToItrEntityMapper _artistCardToItrMapper;

    public CardsByArtistAggregatorService(ILogger logger) : this(
        new ArtistAdapterService(logger),
        new ArtistIdItrToXfrMapper(),
        new ArtistCardExtToItrEntityMapper())
    { }

    private CardsByArtistAggregatorService(
        IArtistAdapterService artistAdapterService,
        IArtistIdItrToXfrMapper aristIdToXfrMapper,
        IArtistCardExtToItrEntityMapper artistCardToItrMapper)
    {
        _artistAdapterService = artistAdapterService;
        _aristIdToXfrMapper = aristIdToXfrMapper;
        _artistCardToItrMapper = artistCardToItrMapper;
    }

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByArtistAsync(IArtistIdItrEntity artistId)
    {
        IArtistIdXfrEntity xfrEntity = await _aristIdToXfrMapper.Map(artistId).ConfigureAwait(false);
        IOperationResponse<IEnumerable<ScryfallArtistCardExtEntity>> adapterResponse = await _artistAdapterService.CardsByArtistIdAsync(xfrEntity).ConfigureAwait(false);

        if (adapterResponse.IsFailure)
        {
            return new FailureOperationResponse<ICardItemCollectionOufEntity>(adapterResponse.OuterException);
        }

        //TODO: Needs ExtToItr mapper
        List<ICardItemItrEntity> mappedCards = [];
        foreach (ScryfallArtistCardExtEntity extEntity in adapterResponse.ResponseData)
        {
            ICardItemItrEntity itrEntity = await _artistCardToItrMapper.Map(extEntity).ConfigureAwait(false);
            mappedCards.Add(itrEntity);
        }

        //TODO: Needs ItrToOuf mapper
        ICardItemCollectionOufEntity collection = new CardItemCollectionOufEntity
        {
            Data = mappedCards
        };

        return new SuccessOperationResponse<ICardItemCollectionOufEntity>(collection);
    }
}
