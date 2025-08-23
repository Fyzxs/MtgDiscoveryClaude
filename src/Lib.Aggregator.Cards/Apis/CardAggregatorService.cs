using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.Cards.Apis;

public sealed class CardAggregatorService : ICardAggregatorService
{
    private readonly ICardAggregatorService _cardAggregatorOperations;

    public CardAggregatorService(ICardAggregatorService cardAggregatorOperations)
    {
        _cardAggregatorOperations = cardAggregatorOperations;
    }

    public Task<OperationResponse<ICardItemCollectionItrEntity>> CardsByIdsAsync(ICardIdsItrEntity args)
    {
        return _cardAggregatorOperations.CardsByIdsAsync(args);
    }
}
