using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Cards.Apis;
using Lib.Adapter.Cards.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.SetCards;
using Lib.Aggregator.Cards.Exceptions;
using Lib.Aggregator.Cards.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.Cards;
using Lib.Shared.DataModels.Entities.Itrs.Sets;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Cards.Queries;

internal sealed class CardsBySetCodeAggregatorService : ICardsBySetCodeAggregatorService
{
    private readonly ICardAdapterService _cardAdapterService;
    private readonly ISetCodeItrToXfrMapper _setCodeItrToXfrMapper;
    private readonly ISetCardItemCollectionExtToOufMapper _collectionMapper;

    public CardsBySetCodeAggregatorService(ILogger logger) : this(
        new CardAdapterService(logger),
        new SetCodeItrToXfrMapper(),
        new SetCardItemCollectionExtToOufMapper())
    { }

    private CardsBySetCodeAggregatorService(
        ICardAdapterService cardAdapterService,
        ISetCodeItrToXfrMapper setCodeItrToXfrMapper,
        ISetCardItemCollectionExtToOufMapper collectionMapper)
    {
        _cardAdapterService = cardAdapterService;
        _setCodeItrToXfrMapper = setCodeItrToXfrMapper;
        _collectionMapper = collectionMapper;
    }

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> Execute(
        ISetCodeItrEntity input,
        CancellationToken cancellationToken)
    {
        ISetCodeXfrEntity xfrEntity = await _setCodeItrToXfrMapper.Map(input).ConfigureAwait(false);
        IOperationResponse<IEnumerable<ScryfallSetCardItemExtEntity>> response = await _cardAdapterService.GetCardsBySetCodeAsync(xfrEntity, cancellationToken).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICardItemCollectionOufEntity>(new CardAggregatorOperationException($"Failed to retrieve cards for set '{input.SetCode}'", response.OuterException));
        }

        ICardItemCollectionOufEntity oufEntity = await _collectionMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<ICardItemCollectionOufEntity>(oufEntity);
    }
}
