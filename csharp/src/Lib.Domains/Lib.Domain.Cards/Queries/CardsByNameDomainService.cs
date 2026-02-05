using System.Threading;
using System.Threading.Tasks;
using Lib.Aggregator.Cards.Apis;
using Lib.Shared.DataModels.Entities.Itrs.Cards;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.Cards.Queries;

/// <summary>
/// Single-method service for retrieving cards by name.
/// Delegates to aggregator layer for data retrieval.
/// </summary>
internal sealed class CardsByNameDomainService : ICardsByNameDomainService
{
    private readonly ICardAggregatorService _cardAggregatorService;

    public CardsByNameDomainService(ILogger logger) : this(new CardAggregatorService(logger))
    { }

    private CardsByNameDomainService(ICardAggregatorService cardAggregatorService) => _cardAggregatorService = cardAggregatorService;

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> Execute(
        ICardNameItrEntity input,
        CancellationToken cancellationToken)
        => await _cardAggregatorService.CardsByNameAsync(input, cancellationToken).ConfigureAwait(false);
}
