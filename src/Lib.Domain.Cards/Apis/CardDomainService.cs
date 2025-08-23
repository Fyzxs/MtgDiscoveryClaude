using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Operations;

namespace Lib.Domain.Cards.Apis;

public sealed class CardDomainService : ICardDomainService
{
    private readonly ICardDomainService _cardDomainOperations;

    public CardDomainService(ICardDomainService cardDomainOperations)
    {
        _cardDomainOperations = cardDomainOperations;
    }

    public Task<OperationStatus> CardsByIdsAsync(ICardIdsItrEntity args)
    {
        return _cardDomainOperations.CardsByIdsAsync(args);
    }
}
