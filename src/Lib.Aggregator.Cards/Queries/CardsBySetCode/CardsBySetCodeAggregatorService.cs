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

namespace Lib.Aggregator.Cards.Queries.CardsBySetCode;

internal sealed class CardsBySetCodeAggregatorService : ICardsBySetCodeAggregatorService
{
    private readonly ICardAdapterService _cardAdapterService;
    private readonly ICollectionSetCardItemExtToItrMapper _setCardItemMapper;
    private readonly ISetCodeItrToXfrMapper _setCodeItrToXfrMapper;
    private readonly ICollectionCardItemItrToOufMapper _cardItemItrToOufMapper;

    public CardsBySetCodeAggregatorService(ILogger logger) : this(
        new CardAdapterService(logger),
        new CollectionSetCardItemExtToItrMapper(),
        new SetCodeItrToXfrMapper(),
        new CollectionCardItemItrToOufMapper())
    { }

    private CardsBySetCodeAggregatorService(
        ICardAdapterService cardAdapterService,
        ICollectionSetCardItemExtToItrMapper setCardItemMapper,
        ISetCodeItrToXfrMapper setCodeItrToXfrMapper,
        ICollectionCardItemItrToOufMapper cardItemItrToOufMapper)
    {
        _cardAdapterService = cardAdapterService;
        _setCardItemMapper = setCardItemMapper;
        _setCodeItrToXfrMapper = setCodeItrToXfrMapper;
        _cardItemItrToOufMapper = cardItemItrToOufMapper;
    }

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> Execute(ISetCodeItrEntity input)
    {
        ISetCodeXfrEntity xfrEntity = await _setCodeItrToXfrMapper.Map(input).ConfigureAwait(false);
        IOperationResponse<IEnumerable<ScryfallSetCardItemExtEntity>> response = await _cardAdapterService.GetCardsBySetCodeAsync(xfrEntity).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICardItemCollectionOufEntity>(new CardAggregatorOperationException($"Failed to retrieve cards for set '{input.SetCode}'", response.OuterException));
        }

        IEnumerable<ICardItemItrEntity> mappedCards = await _setCardItemMapper.Map(response.ResponseData).ConfigureAwait(false);
        ICardItemCollectionOufEntity oufEntity = await _cardItemItrToOufMapper.Map(mappedCards).ConfigureAwait(false);
        return new SuccessOperationResponse<ICardItemCollectionOufEntity>(oufEntity);
    }
}
