using System.Threading.Tasks;
using Lib.Domain.Cards.Queries;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.Cards.Apis;

public sealed class CardDomainService : ICardDomainService
{
    private readonly ICardDomainService _cardDomainOperations;

    public CardDomainService(ILogger logger) : this(new CardDomainOperations(logger))
    {

    }
    private CardDomainService(ICardDomainService cardDomainOperations)
    {
        _cardDomainOperations = cardDomainOperations;
    }

    public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByIdsAsync(ICardIdsItrEntity args)
    {
        return _cardDomainOperations.CardsByIdsAsync(args);
    }
}
