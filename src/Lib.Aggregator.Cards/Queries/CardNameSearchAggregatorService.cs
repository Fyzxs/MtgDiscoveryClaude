using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Cards.Apis;
using Lib.Adapter.Cards.Apis.Entities;
using Lib.Aggregator.Cards.Entities;
using Lib.Aggregator.Cards.Exceptions;
using Lib.Aggregator.Cards.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Cards.Queries;

internal sealed class CardNameSearchAggregatorService : ICardNameSearchAggregatorService
{
    private readonly ICardAdapterService _cardAdapterService;
    private readonly ICardSearchTermItrToXfrMapper _cardSearchTermItrToXfrMapper;
    private readonly ICollectionStringToCardNameSearchResultMapper _stringToSearchResultMapper;

    public CardNameSearchAggregatorService(ILogger logger) : this(
        new CardAdapterService(logger),
        new CardSearchTermItrToXfrMapper(),
        new CollectionStringToCardNameSearchResultMapper())
    { }

    private CardNameSearchAggregatorService(
        ICardAdapterService cardAdapterService,
        ICardSearchTermItrToXfrMapper cardSearchTermItrToXfrMapper,
        ICollectionStringToCardNameSearchResultMapper stringToSearchResultMapper)
    {
        _cardAdapterService = cardAdapterService;
        _cardSearchTermItrToXfrMapper = cardSearchTermItrToXfrMapper;
        _stringToSearchResultMapper = stringToSearchResultMapper;
    }

    public async Task<IOperationResponse<ICardNameSearchCollectionOufEntity>> Execute(ICardSearchTermItrEntity input)
    {
        ICardSearchTermXfrEntity xfrEntity = await _cardSearchTermItrToXfrMapper.Map(input).ConfigureAwait(false);
        IOperationResponse<IEnumerable<string>> response = await _cardAdapterService.SearchCardNamesAsync(xfrEntity).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICardNameSearchCollectionOufEntity>(new CardAggregatorOperationException($"Failed to search for cards with term '{input.SearchTerm}'", response.OuterException));
        }

        ICollection<ICardNameSearchResultItrEntity> results = await _stringToSearchResultMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<ICardNameSearchCollectionOufEntity>(new CardNameSearchCollectionOufEntity { Names = results });
    }
}
