using System.Threading.Tasks;
using Lib.Aggregator.Cards.Queries;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Cards.Apis;

public sealed class CardAggregatorService : ICardAggregatorService
{
    private readonly ICardAggregatorService _cardAggregatorOperations;

    public CardAggregatorService(ILogger logger) : this(new QueryCardAggregatorService(logger))
    {

    }
    private CardAggregatorService(ICardAggregatorService cardAggregatorOperations)
    {
        _cardAggregatorOperations = cardAggregatorOperations;
    }

    public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByIdsAsync(ICardIdsItrEntity args)
    {
        return _cardAggregatorOperations.CardsByIdsAsync(args);
    }

    public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsBySetCodeAsync(ISetCodeItrEntity setCode)
    {
        return _cardAggregatorOperations.CardsBySetCodeAsync(setCode);
    }

    public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByNameAsync(ICardNameItrEntity cardName)
    {
        return _cardAggregatorOperations.CardsByNameAsync(cardName);
    }
}
