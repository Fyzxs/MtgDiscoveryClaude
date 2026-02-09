using System.Threading;
using System.Threading.Tasks;
using Lib.Aggregator.Cards.Apis;
using Lib.Shared.DataModels.Entities.Itrs.Sets;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.Cards.Queries;

/// <summary>
/// Single-method service for retrieving cards by set code.
/// Delegates to aggregator layer for data retrieval.
/// </summary>
internal sealed class CardsBySetCodeDomain : ICardsBySetCodeDomain
{
    private readonly ICardAggregatorService _cardAggregatorService;

    public CardsBySetCodeDomain(ILogger logger) : this(new CardAggregatorService(logger))
    { }

    private CardsBySetCodeDomain(ICardAggregatorService cardAggregatorService) => _cardAggregatorService = cardAggregatorService;

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> Execute(
        ISetCodeItrEntity input,
        CancellationToken cancellationToken)
        => await _cardAggregatorService.CardsBySetCodeAsync(input, cancellationToken).ConfigureAwait(false);
}
