using System.Threading.Tasks;
using Lib.Aggregator.Cards.Apis;
using Lib.Domain.Cards.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.Cards.Queries;

internal sealed class CardsQueryDomainService : ICardsQueryDomainService
{
    private readonly ICardsByIdsDomainService _cardsByIdsService;
    private readonly ICardsBySetCodeDomainService _cardsBySetCodeService;
    private readonly ICardsByNameDomainService _cardsByNameService;
    private readonly ICardNameSearchDomainService _cardNameSearchService;

    public CardsQueryDomainService(ILogger logger) : this(
        new CardsByIdsDomainService(logger),
        new CardsBySetCodeDomainService(logger),
        new CardsByNameDomainService(logger),
        new CardNameSearchDomainService(logger))
    { }

    private CardsQueryDomainService(
        ICardsByIdsDomainService cardsByIdsService,
        ICardsBySetCodeDomainService cardsBySetCodeService,
        ICardsByNameDomainService cardsByNameService,
        ICardNameSearchDomainService cardNameSearchService)
    {
        _cardsByIdsService = cardsByIdsService;
        _cardsBySetCodeService = cardsBySetCodeService;
        _cardsByNameService = cardsByNameService;
        _cardNameSearchService = cardNameSearchService;
    }

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByIdsAsync(ICardIdsItrEntity args)
        => await _cardsByIdsService.Execute(args).ConfigureAwait(false);

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsBySetCodeAsync(ISetCodeItrEntity setCode)
        => await _cardsBySetCodeService.Execute(setCode).ConfigureAwait(false);

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByNameAsync(ICardNameItrEntity cardName)
        => await _cardsByNameService.Execute(cardName).ConfigureAwait(false);

    public async Task<IOperationResponse<ICardNameSearchCollectionOufEntity>> CardNameSearchAsync(ICardSearchTermItrEntity searchTerm)
        => await _cardNameSearchService.Execute(searchTerm).ConfigureAwait(false);
}
