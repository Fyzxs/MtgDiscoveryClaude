using System.Threading;
using System.Threading.Tasks;
using Lib.Aggregator.Cards.Queries;
using Lib.Shared.DataModels.Entities.Itrs.Cards;
using Lib.Shared.DataModels.Entities.Itrs.Sets;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Cards.Apis;

public sealed class CardAggregatorService : ICardAggregatorService
{
    private readonly ICardsQueryAggregatorService _cardAggregatorOperations;

    public CardAggregatorService(ILogger logger) : this(new CardsQueryAggregator(logger))
    { }

    private CardAggregatorService(ICardsQueryAggregatorService cardAggregatorOperations) => _cardAggregatorOperations = cardAggregatorOperations;

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByIdsAsync(
        ICardIdsItrEntity args,
        CancellationToken cancellationToken)
        => await _cardAggregatorOperations.CardsByIdsAsync(args, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsBySetCodeAsync(
        ISetCodeItrEntity setCode,
        CancellationToken cancellationToken)
        => await _cardAggregatorOperations.CardsBySetCodeAsync(setCode, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByNameAsync(
        ICardNameItrEntity cardName,
        CancellationToken cancellationToken)
        => await _cardAggregatorOperations.CardsByNameAsync(cardName, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICardNameSearchCollectionOufEntity>> CardNameSearchAsync(
        ICardSearchTermItrEntity searchTerm,
        CancellationToken cancellationToken)
        => await _cardAggregatorOperations.CardNameSearchAsync(searchTerm, cancellationToken).ConfigureAwait(false);
}
