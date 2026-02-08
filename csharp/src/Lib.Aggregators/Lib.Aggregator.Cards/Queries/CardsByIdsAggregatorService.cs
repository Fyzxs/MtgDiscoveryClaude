using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Cards.Apis;
using Lib.Adapter.Cards.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.CardItems;
using Lib.Aggregator.Cards.Exceptions;
using Lib.Aggregator.Cards.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.Cards;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Cards.Queries;

internal sealed class CardsByIdsAggregatorService : ICardsByIdsAggregatorService
{
    private readonly ICardAdapterService _cardAdapterService;
    private readonly ICardIdsItrToXfrMapper _cardIdsItrToXfrMapper;
    private readonly ICardItemCollectionExtToOufMapper _collectionMapper;

    public CardsByIdsAggregatorService(ILogger logger) : this(
        new CardAdapterService(logger),
        new CardIdsItrToXfrMapper(),
        new CardItemCollectionExtToOufMapper())
    { }

    private CardsByIdsAggregatorService(
        ICardAdapterService cardAdapterService,
        ICardIdsItrToXfrMapper cardIdsItrToXfrMapper,
        ICardItemCollectionExtToOufMapper collectionMapper)
    {
        _cardAdapterService = cardAdapterService;
        _cardIdsItrToXfrMapper = cardIdsItrToXfrMapper;
        _collectionMapper = collectionMapper;
    }

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> Execute(
        ICardIdsItrEntity input,
        CancellationToken cancellationToken)
    {
        ICardIdsXfrEntity xfrEntity = await _cardIdsItrToXfrMapper.Map(input).ConfigureAwait(false);
        IOperationResponse<IEnumerable<ScryfallCardItemExtEntity>> response = await _cardAdapterService.GetCardsByIdsAsync(xfrEntity, cancellationToken).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICardItemCollectionOufEntity>(new CardAggregatorOperationException("Failed to retrieve cards by IDs", response.OuterException));
        }

        ICardItemCollectionOufEntity oufEntity = await _collectionMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<ICardItemCollectionOufEntity>(oufEntity);
    }
}
