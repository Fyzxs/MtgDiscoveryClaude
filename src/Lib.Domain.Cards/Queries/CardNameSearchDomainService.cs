using System.Threading.Tasks;
using Lib.Aggregator.Cards.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.Cards.Queries;

/// <summary>
/// Single-method service for card name search operation.
/// Delegates to aggregator layer for data retrieval.
/// </summary>
internal sealed class CardNameSearchDomainService : ICardNameSearchDomainService
{
    private readonly ICardAggregatorService _cardAggregatorService;

    public CardNameSearchDomainService(ILogger logger) : this(new CardAggregatorService(logger))
    { }

    private CardNameSearchDomainService(ICardAggregatorService cardAggregatorService) => _cardAggregatorService = cardAggregatorService;

    public async Task<IOperationResponse<ICardNameSearchCollectionOufEntity>> Execute(ICardSearchTermItrEntity input) => await _cardAggregatorService.CardNameSearchAsync(input).ConfigureAwait(false);
}
