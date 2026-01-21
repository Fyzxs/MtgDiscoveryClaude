using System.Threading.Tasks;
using Lib.Aggregator.Cards.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.Cards.Queries;

/// <summary>
/// Single-method service for retrieving cards by ID collection.
/// Delegates to aggregator layer for data retrieval.
/// </summary>
internal sealed class CardsByIdsDomainService : ICardsByIdsDomainService
{
    private readonly ICardAggregatorService _cardAggregatorService;

    public CardsByIdsDomainService(ILogger logger) : this(new CardAggregatorService(logger))
    { }

    private CardsByIdsDomainService(ICardAggregatorService cardAggregatorService) => _cardAggregatorService = cardAggregatorService;

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> Execute(ICardIdsItrEntity input) => await _cardAggregatorService.CardsByIdsAsync(input).ConfigureAwait(false);
}
