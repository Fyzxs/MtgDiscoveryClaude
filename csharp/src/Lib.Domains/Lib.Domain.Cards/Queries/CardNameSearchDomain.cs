using System.Threading;
using System.Threading.Tasks;
using Lib.Aggregator.Cards.Apis;
using Lib.Shared.DataModels.Entities.Itrs.Cards;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.Cards.Queries;

/// <summary>
/// Single-method service for card name search operation.
/// Delegates to aggregator layer for data retrieval.
/// </summary>
internal sealed class CardNameSearchDomain : ICardNameSearchDomain
{
    private readonly ICardAggregatorService _cardAggregatorService;

    public CardNameSearchDomain(ILogger logger) : this(new CardAggregatorService(logger))
    { }

    private CardNameSearchDomain(ICardAggregatorService cardAggregatorService) => _cardAggregatorService = cardAggregatorService;

    public async Task<IOperationResponse<ICardNameSearchCollectionOufEntity>> Execute(
        ICardSearchTermItrEntity input,
        CancellationToken cancellationToken)
        => await _cardAggregatorService.CardNameSearchAsync(input, cancellationToken).ConfigureAwait(false);
}
