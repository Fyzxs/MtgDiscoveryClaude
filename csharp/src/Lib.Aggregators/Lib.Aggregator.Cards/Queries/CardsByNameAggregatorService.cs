using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Cards.Apis;
using Lib.Adapter.Cards.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.CardsByName;
using Lib.Aggregator.Cards.Exceptions;
using Lib.Aggregator.Cards.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.Cards;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Cards.Queries;

internal sealed class CardsByNameAggregatorService : ICardsByNameAggregatorService
{
    private readonly ICardAdapterService _cardAdapterService;
    private readonly ICardNameItrToXfrMapper _cardNameItrToXfrMapper;
    private readonly ICardByNameCollectionExtToOufMapper _collectionMapper;

    public CardsByNameAggregatorService(ILogger logger) : this(
        new CardAdapterService(logger),
        new CardNameItrToXfrMapper(),
        new CardByNameCollectionExtToOufMapper())
    { }

    private CardsByNameAggregatorService(
        ICardAdapterService cardAdapterService,
        ICardNameItrToXfrMapper cardNameItrToXfrMapper,
        ICardByNameCollectionExtToOufMapper collectionMapper)
    {
        _cardAdapterService = cardAdapterService;
        _cardNameItrToXfrMapper = cardNameItrToXfrMapper;
        _collectionMapper = collectionMapper;
    }

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> Execute(
        ICardNameItrEntity input,
        CancellationToken cancellationToken)
    {
        ICardNameXfrEntity xfrEntity = await _cardNameItrToXfrMapper.Map(input).ConfigureAwait(false);
        IOperationResponse<IEnumerable<ScryfallCardByNameExtEntity>> response = await _cardAdapterService.GetCardsByNameAsync(xfrEntity, cancellationToken).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICardItemCollectionOufEntity>(new CardAggregatorOperationException($"Failed to retrieve cards for name '{input.CardName}'", response.OuterException));
        }

        ICardItemCollectionOufEntity oufEntity = await _collectionMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<ICardItemCollectionOufEntity>(oufEntity);
    }
}
