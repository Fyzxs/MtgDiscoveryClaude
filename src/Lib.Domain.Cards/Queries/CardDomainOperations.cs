using System.Threading.Tasks;
using Lib.Aggregator.Cards.Apis;
using Lib.Domain.Cards.Apis;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Operations;

namespace Lib.Domain.Cards.Queries;

internal sealed class CardDomainOperations : ICardDomainService
{
    private readonly ICardAggregatorService _cardAggregatorService;

    public CardDomainOperations(ICardAggregatorService cardAggregatorService)
    {
        _cardAggregatorService = cardAggregatorService;
    }

    public async Task<OperationStatus> CardsByIdsAsync(ICardIdsItrEntity args)
    {
        return await _cardAggregatorService.CardsByIdsAsync(args).ConfigureAwait(false);
    }
}
