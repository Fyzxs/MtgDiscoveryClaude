using System.Threading.Tasks;
using Lib.Domain.Cards.Apis;
using Lib.MtgDiscovery.Data.Apis;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Data.Queries;

internal sealed class CardDataService : ICardDataService
{
    private readonly ICardDomainService _cardDomainService;

    public CardDataService(ILogger logger) : this(new CardDomainService(logger))
    { }

    public CardDataService(ICardDomainService cardDomainService) => _cardDomainService = cardDomainService;

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByIdsAsync(ICardIdsItrEntity args) => await _cardDomainService.CardsByIdsAsync(args).ConfigureAwait(false);

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsBySetCodeAsync(ISetCodeItrEntity setCode) => await _cardDomainService.CardsBySetCodeAsync(setCode).ConfigureAwait(false);

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByNameAsync(ICardNameItrEntity cardName) => await _cardDomainService.CardsByNameAsync(cardName).ConfigureAwait(false);

    public async Task<IOperationResponse<ICardNameSearchResultCollectionItrEntity>> CardNameSearchAsync(ICardSearchTermItrEntity searchTerm) => await _cardDomainService.CardNameSearchAsync(searchTerm).ConfigureAwait(false);
}
