using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Cards.Apis;
using Lib.Adapter.Cards.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Aggregator.Cards.Exceptions;
using Lib.Aggregator.Cards.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Cards.Queries;

internal sealed class CardsByIdsAggregatorService : ICardsByIdsAggregatorService
{
    private readonly ICardAdapterService _cardAdapterService;
    private readonly ICollectionCardItemExtToItrMapper _cardItemMapper;
    private readonly ICardIdsItrToXfrMapper _cardIdsItrToXfrMapper;
    private readonly ICollectionCardItemItrToOufMapper _cardItemItrToOufMapper;

    public CardsByIdsAggregatorService(ILogger logger) : this(
        new CardAdapterService(logger),
        new CollectionCardItemExtToItrMapper(),
        new CardIdsItrToXfrMapper(),
        new CollectionCardItemItrToOufMapper())
    { }

    private CardsByIdsAggregatorService(
        ICardAdapterService cardAdapterService,
        ICollectionCardItemExtToItrMapper cardItemMapper,
        ICardIdsItrToXfrMapper cardIdsItrToXfrMapper,
        ICollectionCardItemItrToOufMapper cardItemItrToOufMapper)
    {
        _cardAdapterService = cardAdapterService;
        _cardItemMapper = cardItemMapper;
        _cardIdsItrToXfrMapper = cardIdsItrToXfrMapper;
        _cardItemItrToOufMapper = cardItemItrToOufMapper;
    }

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> Execute(ICardIdsItrEntity input)
    {
        ICardIdsXfrEntity xfrEntity = await _cardIdsItrToXfrMapper.Map(input).ConfigureAwait(false);
        IOperationResponse<IEnumerable<ScryfallCardItemExtEntity>> response = await _cardAdapterService.GetCardsByIdsAsync(xfrEntity).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICardItemCollectionOufEntity>(new CardAggregatorOperationException("Failed to retrieve cards by IDs", response.OuterException));
        }

        //TODO: These mappers shouldn't follow each other. They should get combined. Applies to many aggregators
        IEnumerable<ICardItemItrEntity> mappedCards = await _cardItemMapper.Map(response.ResponseData).ConfigureAwait(false);
        ICardItemCollectionOufEntity oufEntity = await _cardItemItrToOufMapper.Map(mappedCards).ConfigureAwait(false);
        return new SuccessOperationResponse<ICardItemCollectionOufEntity>(oufEntity);
    }
}
