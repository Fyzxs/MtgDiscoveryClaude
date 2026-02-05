using System.Threading;
using System.Threading.Tasks;
using Lib.Domain.Cards.Apis;
using Lib.Shared.DataModels.Entities.Itrs.Cards;
using Lib.Shared.DataModels.Entities.Itrs.Sets;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
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

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByIdsAsync(
        ICardIdsItrEntity args,
        CancellationToken cancellationToken)
        => await _cardsByIdsService.Execute(args, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsBySetCodeAsync(
        ISetCodeItrEntity setCode,
        CancellationToken cancellationToken)
        => await _cardsBySetCodeService.Execute(setCode, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByNameAsync(
        ICardNameItrEntity cardName,
        CancellationToken cancellationToken)
        => await _cardsByNameService.Execute(cardName, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICardNameSearchCollectionOufEntity>> CardNameSearchAsync(
        ICardSearchTermItrEntity searchTerm,
        CancellationToken cancellationToken)
        => await _cardNameSearchService.Execute(searchTerm, cancellationToken).ConfigureAwait(false);
}
