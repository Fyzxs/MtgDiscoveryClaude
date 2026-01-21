using System.Threading.Tasks;
using Lib.Aggregator.Cards.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.Cards.Queries;

/// <summary>
/// Single-method service for retrieving cards by set code.
/// Delegates to aggregator layer for data retrieval.
/// </summary>
internal sealed class CardsBySetCodeDomainService : ICardsBySetCodeDomainService
{
    private readonly ICardAggregatorService _cardAggregatorService;

    public CardsBySetCodeDomainService(ICardAggregatorService cardAggregatorService) => _cardAggregatorService = cardAggregatorService;

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> Execute(ISetCodeItrEntity input) => await _cardAggregatorService.CardsBySetCodeAsync(input).ConfigureAwait(false);
}
