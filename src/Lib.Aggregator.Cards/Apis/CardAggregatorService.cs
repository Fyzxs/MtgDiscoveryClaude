using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Operations;

namespace Lib.Aggregator.Cards.Apis;

public sealed class CardAggregatorService : ICardAggregatorService
{
    private readonly ICardAggregatorService _cardAggregatorOperations;

    public CardAggregatorService(ICardAggregatorService cardAggregatorOperations)
    {
        _cardAggregatorOperations = cardAggregatorOperations;
    }

    public Task<OperationStatus> CardsByIdsAsync(ICardIdsItrEntity args)
    {
        return _cardAggregatorOperations.CardsByIdsAsync(args);
    }
}
