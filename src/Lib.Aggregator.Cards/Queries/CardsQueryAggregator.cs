using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Cards.Apis;
using Lib.Adapter.Cards.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Aggregator.Cards.Apis;
using Lib.Aggregator.Cards.Entities;
using Lib.Aggregator.Cards.Exceptions;
using Lib.Aggregator.Cards.Queries.Mappers;
using Lib.Aggregator.Scryfall.Shared.Entities;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Cards.Queries;

internal sealed class CardsQueryAggregator : ICardAggregatorService
{
    private readonly ICardAdapterService _cardAdapterService;
    private readonly ICollectionCardItemExtToItrMapper _cardItemMapper;
    private readonly ICollectionSetCardItemExtToItrMapper _setCardItemMapper;
    private readonly ICollectionCardByNameExtToItrMapper _cardByNameMapper;
    private readonly ICardIdsItrToXfrMapper _cardIdsItrToXfrMapper;
    private readonly ISetCodeItrToXfrMapper _setCodeItrToXfrMapper;
    private readonly ICardNameItrToXfrMapper _cardNameItrToXfrMapper;
    private readonly ICardSearchTermItrToXfrMapper _cardSearchTermItrToXfrMapper;
    private readonly ICollectionStringToCardNameSearchResultMapper _stringToSearchResultMapper;
    private readonly ICollectionCardItemItrToOufMapper _cardItemItrToOufMapper;

    public CardsQueryAggregator(ILogger logger) : this(
        new CardAdapterService(logger),
        new CollectionCardItemExtToItrMapper(),
        new CollectionSetCardItemExtToItrMapper(),
        new CollectionCardByNameExtToItrMapper(),
        new CardIdsItrToXfrMapper(),
        new SetCodeItrToXfrMapper(),
        new CardNameItrToXfrMapper(),
        new CardSearchTermItrToXfrMapper(),
        new CollectionStringToCardNameSearchResultMapper(),
        new CollectionCardItemItrToOufMapper())
    { }

    private CardsQueryAggregator(
        ICardAdapterService cardAdapterService,
        ICollectionCardItemExtToItrMapper cardItemMapper,
        ICollectionSetCardItemExtToItrMapper setCardItemMapper,
        ICollectionCardByNameExtToItrMapper cardByNameMapper,
        ICardIdsItrToXfrMapper cardIdsItrToXfrMapper,
        ISetCodeItrToXfrMapper setCodeItrToXfrMapper,
        ICardNameItrToXfrMapper cardNameItrToXfrMapper,
        ICardSearchTermItrToXfrMapper cardSearchTermItrToXfrMapper,
        ICollectionStringToCardNameSearchResultMapper stringToSearchResultMapper,
        ICollectionCardItemItrToOufMapper cardItemItrToOufMapper)
    {
        _cardAdapterService = cardAdapterService;
        _cardItemMapper = cardItemMapper;
        _setCardItemMapper = setCardItemMapper;
        _cardByNameMapper = cardByNameMapper;
        _cardIdsItrToXfrMapper = cardIdsItrToXfrMapper;
        _setCodeItrToXfrMapper = setCodeItrToXfrMapper;
        _cardNameItrToXfrMapper = cardNameItrToXfrMapper;
        _cardSearchTermItrToXfrMapper = cardSearchTermItrToXfrMapper;
        _stringToSearchResultMapper = stringToSearchResultMapper;
        _cardItemItrToOufMapper = cardItemItrToOufMapper;
    }

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByIdsAsync(ICardIdsItrEntity args)
    {
        ICardIdsXfrEntity xfrEntity = await _cardIdsItrToXfrMapper.Map(args).ConfigureAwait(false);
        IOperationResponse<IEnumerable<ScryfallCardItemExtEntity>> response = await _cardAdapterService.GetCardsByIdsAsync(xfrEntity).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICardItemCollectionOufEntity>(new CardAggregatorOperationException("Failed to retrieve cards by IDs", response.OuterException));
        }

        IEnumerable<ICardItemItrEntity> mappedCards = await _cardItemMapper.Map(response.ResponseData).ConfigureAwait(false);
        ICardItemCollectionOufEntity oufEntity = await _cardItemItrToOufMapper.Map(mappedCards).ConfigureAwait(false);
        return new SuccessOperationResponse<ICardItemCollectionOufEntity>(oufEntity);
    }

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsBySetCodeAsync(ISetCodeItrEntity setCode)
    {
        ISetCodeXfrEntity xfrEntity = await _setCodeItrToXfrMapper.Map(setCode).ConfigureAwait(false);
        IOperationResponse<IEnumerable<ScryfallSetCardItemExtEntity>> response = await _cardAdapterService.GetCardsBySetCodeAsync(xfrEntity).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICardItemCollectionOufEntity>(new CardAggregatorOperationException($"Failed to retrieve cards for set '{setCode.SetCode}'", response.OuterException));
        }

        IEnumerable<ICardItemItrEntity> mappedCards = await _setCardItemMapper.Map(response.ResponseData).ConfigureAwait(false);
        ICardItemCollectionOufEntity oufEntity = await _cardItemItrToOufMapper.Map(mappedCards).ConfigureAwait(false);
        return new SuccessOperationResponse<ICardItemCollectionOufEntity>(oufEntity);
    }

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByNameAsync(ICardNameItrEntity cardName)
    {
        ICardNameXfrEntity xfrEntity = await _cardNameItrToXfrMapper.Map(cardName).ConfigureAwait(false);
        IOperationResponse<IEnumerable<ScryfallCardByNameExtEntity>> response = await _cardAdapterService.GetCardsByNameAsync(xfrEntity).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICardItemCollectionOufEntity>(new CardAggregatorOperationException($"Failed to retrieve cards for name '{cardName.CardName}'", response.OuterException));
        }

        IEnumerable<ICardItemItrEntity> mappedCards = await _cardByNameMapper.Map(response.ResponseData).ConfigureAwait(false);
        ICardItemCollectionOufEntity oufEntity = await _cardItemItrToOufMapper.Map(mappedCards).ConfigureAwait(false);
        return new SuccessOperationResponse<ICardItemCollectionOufEntity>(oufEntity);
    }

    public async Task<IOperationResponse<ICardNameSearchCollectionOufEntity>> CardNameSearchAsync(ICardSearchTermItrEntity searchTerm)
    {
        ICardSearchTermXfrEntity xfrEntity = await _cardSearchTermItrToXfrMapper.Map(searchTerm).ConfigureAwait(false);
        IOperationResponse<IEnumerable<string>> response = await _cardAdapterService.SearchCardNamesAsync(xfrEntity).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICardNameSearchCollectionOufEntity>(new CardAggregatorOperationException($"Failed to search for cards with term '{searchTerm.SearchTerm}'", response.OuterException));
        }

        ICollection<ICardNameSearchResultItrEntity> results = await _stringToSearchResultMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<ICardNameSearchCollectionOufEntity>(new CardNameSearchCollectionOufEntity { Names = results });
    }
}
