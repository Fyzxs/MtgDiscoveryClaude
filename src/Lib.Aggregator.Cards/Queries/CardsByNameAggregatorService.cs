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

internal sealed class CardsByNameAggregatorService : ICardsByNameAggregatorService
{
    private readonly ICardAdapterService _cardAdapterService;
    private readonly ICollectionCardByNameExtToItrMapper _cardByNameMapper;
    private readonly ICardNameItrToXfrMapper _cardNameItrToXfrMapper;
    private readonly ICollectionCardItemItrToOufMapper _cardItemItrToOufMapper;

    public CardsByNameAggregatorService(ILogger logger) : this(
        new CardAdapterService(logger),
        new CollectionCardByNameExtToItrMapper(),
        new CardNameItrToXfrMapper(),
        new CollectionCardItemItrToOufMapper())
    { }

    private CardsByNameAggregatorService(
        ICardAdapterService cardAdapterService,
        ICollectionCardByNameExtToItrMapper cardByNameMapper,
        ICardNameItrToXfrMapper cardNameItrToXfrMapper,
        ICollectionCardItemItrToOufMapper cardItemItrToOufMapper)
    {
        _cardAdapterService = cardAdapterService;
        _cardByNameMapper = cardByNameMapper;
        _cardNameItrToXfrMapper = cardNameItrToXfrMapper;
        _cardItemItrToOufMapper = cardItemItrToOufMapper;
    }

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> Execute(ICardNameItrEntity input)
    {
        ICardNameXfrEntity xfrEntity = await _cardNameItrToXfrMapper.Map(input).ConfigureAwait(false);
        IOperationResponse<IEnumerable<ScryfallCardByNameExtEntity>> response = await _cardAdapterService.GetCardsByNameAsync(xfrEntity).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICardItemCollectionOufEntity>(new CardAggregatorOperationException($"Failed to retrieve cards for name '{input.CardName}'", response.OuterException));
        }

        IEnumerable<ICardItemItrEntity> mappedCards = await _cardByNameMapper.Map(response.ResponseData).ConfigureAwait(false);
        ICardItemCollectionOufEntity oufEntity = await _cardItemItrToOufMapper.Map(mappedCards).ConfigureAwait(false);
        return new SuccessOperationResponse<ICardItemCollectionOufEntity>(oufEntity);
    }
}
