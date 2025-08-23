using System.Threading.Tasks;
using Lib.Domain.Cards.Apis;
using Lib.MtgDiscovery.Data.Apis;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Operations;

namespace Lib.MtgDiscovery.Data.Queries;

internal sealed class CardDataService : ICardDataService
{
    private readonly ICardDomainService _cardDomainService;

    public CardDataService(ICardDomainService cardDomainService)
    {
        _cardDomainService = cardDomainService;
    }

    public async Task<OperationStatus> CardsByIdsAsync(ICardIdsItrEntity args)
    {
        return await _cardDomainService.CardsByIdsAsync(args).ConfigureAwait(false);
    }
}
