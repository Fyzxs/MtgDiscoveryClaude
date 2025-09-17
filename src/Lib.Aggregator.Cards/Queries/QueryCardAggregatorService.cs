using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Cards.Apis;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Aggregator.Cards.Apis;
using Lib.Aggregator.Cards.Entities;
using Lib.Aggregator.Cards.Exceptions;
using Lib.Aggregator.Cards.Queries.Mappers;
using Lib.Aggregator.Scryfall.Shared.Entities;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Cards.Queries;

internal sealed class QueryCardAggregatorService : ICardAggregatorService
{
    private readonly ICardAdapterService _cardAdapterService;
    private readonly ICollectionCardItemExtToItrMapper _cardItemMapper;
    private readonly ICollectionSetCardItemExtToItrMapper _setCardItemMapper;
    private readonly ICollectionCardByNameExtToItrMapper _cardByNameMapper;

    public QueryCardAggregatorService(ILogger logger) : this(
        new CardAdapterService(logger),
        new CollectionCardItemExtToItrMapper(),
        new CollectionSetCardItemExtToItrMapper(),
        new CollectionCardByNameExtToItrMapper())
    { }

    private QueryCardAggregatorService(
        ICardAdapterService cardAdapterService,
        ICollectionCardItemExtToItrMapper cardItemMapper,
        ICollectionSetCardItemExtToItrMapper setCardItemMapper,
        ICollectionCardByNameExtToItrMapper cardByNameMapper)
    {
        _cardAdapterService = cardAdapterService;
        _cardItemMapper = cardItemMapper;
        _setCardItemMapper = setCardItemMapper;
        _cardByNameMapper = cardByNameMapper;
    }

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByIdsAsync(ICardIdsItrEntity args)
    {
        IOperationResponse<IEnumerable<ScryfallCardItemExtEntity>> response = await _cardAdapterService.GetCardsByIdsAsync(args).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICardItemCollectionItrEntity>(new CardAggregatorOperationException("Failed to retrieve cards by IDs", response.OuterException));
        }

        IEnumerable<ICardItemItrEntity> mappedCards = await _cardItemMapper.Map(response.ResponseData).ConfigureAwait(false);
        ICollection<ICardItemItrEntity> cards = [.. mappedCards];
        return new SuccessOperationResponse<ICardItemCollectionItrEntity>(new CardItemCollectionItrEntity { Data = cards });
    }

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsBySetCodeAsync(ISetCodeItrEntity setCode)
    {
        IOperationResponse<IEnumerable<ScryfallSetCardItemExtEntity>> response = await _cardAdapterService.GetCardsBySetCodeAsync(setCode).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICardItemCollectionItrEntity>(new CardAggregatorOperationException($"Failed to retrieve cards for set '{setCode.SetCode}'", response.OuterException));
        }

        IEnumerable<ICardItemItrEntity> mappedCards = await _setCardItemMapper.Map(response.ResponseData).ConfigureAwait(false);
        ICollection<ICardItemItrEntity> cards = [.. mappedCards];
        return new SuccessOperationResponse<ICardItemCollectionItrEntity>(new CardItemCollectionItrEntity { Data = cards });
    }

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByNameAsync(ICardNameItrEntity cardName)
    {
        IOperationResponse<IEnumerable<ScryfallCardByNameExtEntity>> response = await _cardAdapterService.GetCardsByNameAsync(cardName).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICardItemCollectionItrEntity>(new CardAggregatorOperationException($"Failed to retrieve cards for name '{cardName.CardName}'", response.OuterException));
        }

        IEnumerable<ICardItemItrEntity> mappedCards = await _cardByNameMapper.Map(response.ResponseData).ConfigureAwait(false);
        ICollection<ICardItemItrEntity> cards = [.. mappedCards];
        return new SuccessOperationResponse<ICardItemCollectionItrEntity>(new CardItemCollectionItrEntity { Data = cards });
    }

    public async Task<IOperationResponse<ICardNameSearchResultCollectionItrEntity>> CardNameSearchAsync(ICardSearchTermItrEntity searchTerm)
    {
        IOperationResponse<IEnumerable<string>> response = await _cardAdapterService.SearchCardNamesAsync(searchTerm).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICardNameSearchResultCollectionItrEntity>(new CardAggregatorOperationException($"Failed to search for cards with term '{searchTerm.SearchTerm}'", response.OuterException));
        }

        List<ICardNameSearchResultItrEntity> results = [.. response.ResponseData.Select(x => new CardNameSearchResultItrEntity { Name = x }).Cast<ICardNameSearchResultItrEntity>()];
        return new SuccessOperationResponse<ICardNameSearchResultCollectionItrEntity>(new CardNameSearchResultCollectionItrEntity { Names = results });
    }
}
