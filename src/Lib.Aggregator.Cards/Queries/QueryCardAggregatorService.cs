using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Cards.Apis;
using Lib.Aggregator.Cards.Apis;
using Lib.Aggregator.Cards.Entities;
using Lib.Aggregator.Cards.Exceptions;
using Lib.Aggregator.Scryfall.Shared.Entities;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Cards.Queries;

internal sealed class QueryCardAggregatorService : ICardAggregatorService
{
    private readonly ICardAdapterService _cardAdapterService;

    public QueryCardAggregatorService(ILogger logger) : this(new CardAdapterService(logger))
    { }

    private QueryCardAggregatorService(
        ICardAdapterService cardAdapterService)
    {
        _cardAdapterService = cardAdapterService;
    }

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByIdsAsync(ICardIdsItrEntity args)
    {
        IOperationResponse<IEnumerable<ICardItemItrEntity>> response = await _cardAdapterService.GetCardsByIdsAsync(args).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICardItemCollectionItrEntity>(new CardAggregatorOperationException("Failed to retrieve cards by IDs", response.OuterException));
        }

        ICollection<ICardItemItrEntity> cards = response.ResponseData.ToList();
        return new SuccessOperationResponse<ICardItemCollectionItrEntity>(new CardItemCollectionItrEntity { Data = cards });
    }

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsBySetCodeAsync(ISetCodeItrEntity setCode)
    {
        IOperationResponse<IEnumerable<ICardItemItrEntity>> response = await _cardAdapterService.GetCardsBySetCodeAsync(setCode).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICardItemCollectionItrEntity>(new CardAggregatorOperationException($"Failed to retrieve cards for set '{setCode.SetCode}'", response.OuterException));
        }

        ICollection<ICardItemItrEntity> cards = response.ResponseData.ToList();
        return new SuccessOperationResponse<ICardItemCollectionItrEntity>(new CardItemCollectionItrEntity { Data = cards });
    }

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByNameAsync(ICardNameItrEntity cardName)
    {
        IOperationResponse<IEnumerable<ICardItemItrEntity>> response = await _cardAdapterService.GetCardsByNameAsync(cardName).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICardItemCollectionItrEntity>(new CardAggregatorOperationException($"Failed to retrieve cards for name '{cardName.CardName}'", response.OuterException));
        }

        ICollection<ICardItemItrEntity> cards = response.ResponseData.ToList();
        return new SuccessOperationResponse<ICardItemCollectionItrEntity>(new CardItemCollectionItrEntity { Data = cards });
    }

    public async Task<IOperationResponse<ICardNameSearchResultCollectionItrEntity>> CardNameSearchAsync(ICardSearchTermItrEntity searchTerm)
    {
        IOperationResponse<IEnumerable<string>> response = await _cardAdapterService.SearchCardNamesAsync(searchTerm).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICardNameSearchResultCollectionItrEntity>(new CardAggregatorOperationException($"Failed to search for cards with term '{searchTerm.SearchTerm}'", response.OuterException));
        }

        List<ICardNameSearchResultItrEntity> results = response.ResponseData.Select(x => new CardNameSearchResultItrEntity { Name = x }).Cast<ICardNameSearchResultItrEntity>().ToList();
        return new SuccessOperationResponse<ICardNameSearchResultCollectionItrEntity>(new CardNameSearchResultCollectionItrEntity { Names = results });
    }
}
