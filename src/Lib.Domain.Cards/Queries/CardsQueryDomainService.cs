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

    public CardsQueryDomainService(ILogger logger)
        : this(new CardAggregatorService(logger))
    { }

    private CardsQueryDomainService(ICardAggregatorService cardAggregatorService)
    {
        _cardsByIdsService = new CardsByIdsDomainService(cardAggregatorService);
        _cardsBySetCodeService = new CardsBySetCodeDomainService(cardAggregatorService);
        _cardsByNameService = new CardsByNameDomainService(cardAggregatorService);
        _cardNameSearchService = new CardNameSearchDomainService(cardAggregatorService);
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
