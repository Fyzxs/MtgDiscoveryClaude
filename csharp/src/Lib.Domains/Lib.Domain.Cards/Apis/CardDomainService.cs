using System.Threading;
using System.Threading.Tasks;
using Lib.Domain.Cards.Queries;
using Lib.Shared.DataModels.Entities.Itrs.Cards;
using Lib.Shared.DataModels.Entities.Itrs.Sets;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.Cards.Apis;

public sealed class CardDomainService : ICardDomainService
{
    private readonly ICardsQueryDomainService _cardDomainOperations;

    public CardDomainService(ILogger logger) : this(new CardsQueryDomainService(logger))
    { }

    private CardDomainService(ICardsQueryDomainService cardDomainOperations) => _cardDomainOperations = cardDomainOperations;

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByIdsAsync(
        ICardIdsItrEntity args,
        CancellationToken cancellationToken)
        => await _cardDomainOperations.CardsByIdsAsync(args, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsBySetCodeAsync(
        ISetCodeItrEntity setCode,
        CancellationToken cancellationToken)
        => await _cardDomainOperations.CardsBySetCodeAsync(setCode, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByNameAsync(
        ICardNameItrEntity cardName,
        CancellationToken cancellationToken)
        => await _cardDomainOperations.CardsByNameAsync(cardName, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICardNameSearchCollectionOufEntity>> CardNameSearchAsync(
        ICardSearchTermItrEntity searchTerm,
        CancellationToken cancellationToken)
        => await _cardDomainOperations.CardNameSearchAsync(searchTerm, cancellationToken).ConfigureAwait(false);
}
