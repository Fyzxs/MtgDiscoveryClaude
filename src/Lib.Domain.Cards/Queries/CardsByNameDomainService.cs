using System.Threading.Tasks;
using Lib.Aggregator.Cards.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.Cards.Queries;

/// <summary>
/// Single-method service for retrieving cards by name.
/// Delegates to aggregator layer for data retrieval.
/// </summary>
internal sealed class CardsByNameDomainService : ICardsByNameDomainService
{
    private readonly ICardAggregatorService _cardAggregatorService;

    public CardsByNameDomainService(ICardAggregatorService cardAggregatorService) => _cardAggregatorService = cardAggregatorService;

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> Execute(ICardNameItrEntity input) => await _cardAggregatorService.CardsByNameAsync(input).ConfigureAwait(false);
}
