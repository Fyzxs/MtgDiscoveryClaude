using System.Threading.Tasks;
using Lib.Domain.Cards.Queries;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.Cards.Apis;

public sealed class CardDomainService : ICardDomainService
{
    private readonly ICardDomainService _cardDomainOperations;

    public CardDomainService(ILogger logger) : this(new QueryCardDomainService(logger))
    { }

    private CardDomainService(ICardDomainService cardDomainOperations) => _cardDomainOperations = cardDomainOperations;

    public Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByIdsAsync(ICardIdsItrEntity args) => _cardDomainOperations.CardsByIdsAsync(args);

    public Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsBySetCodeAsync(ISetCodeItrEntity setCode) => _cardDomainOperations.CardsBySetCodeAsync(setCode);

    public Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByNameAsync(ICardNameItrEntity cardName) => _cardDomainOperations.CardsByNameAsync(cardName);

    public Task<IOperationResponse<ICardNameSearchCollectionOufEntity>> CardNameSearchAsync(ICardSearchTermItrEntity searchTerm) => _cardDomainOperations.CardNameSearchAsync(searchTerm);
}
