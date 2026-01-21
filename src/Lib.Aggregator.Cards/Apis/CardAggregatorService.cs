using System.Threading.Tasks;
using Lib.Aggregator.Cards.Queries;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Cards.Apis;

public sealed class CardAggregatorService : ICardAggregatorService
{
    private readonly ICardAggregatorService _cardAggregatorOperations;

    public CardAggregatorService(ILogger logger) : this(new CardsQueryAggregator(logger))
    { }

    private CardAggregatorService(ICardAggregatorService cardAggregatorOperations) => _cardAggregatorOperations = cardAggregatorOperations;

    public Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByIdsAsync(ICardIdsItrEntity args) => _cardAggregatorOperations.CardsByIdsAsync(args);

    public Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsBySetCodeAsync(ISetCodeItrEntity setCode) => _cardAggregatorOperations.CardsBySetCodeAsync(setCode);

    public Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByNameAsync(ICardNameItrEntity cardName) => _cardAggregatorOperations.CardsByNameAsync(cardName);

    public Task<IOperationResponse<ICardNameSearchCollectionOufEntity>> CardNameSearchAsync(ICardSearchTermItrEntity searchTerm) => _cardAggregatorOperations.CardNameSearchAsync(searchTerm);
}
