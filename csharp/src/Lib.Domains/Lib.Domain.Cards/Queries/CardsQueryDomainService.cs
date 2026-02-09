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
    private readonly ICardsByIdsDomain _cardsByIds;
    private readonly ICardsBySetCodeDomain _cardsBySetCode;
    private readonly ICardsByNameDomain _cardsByName;
    private readonly ICardNameSearchDomain _cardNameSearch;

    public CardsQueryDomainService(ILogger logger) : this(
        new CardsByIdsDomain(logger),
        new CardsBySetCodeDomain(logger),
        new CardsByNameDomain(logger),
        new CardNameSearchDomain(logger))
    { }

    private CardsQueryDomainService(
        ICardsByIdsDomain cardsByIds,
        ICardsBySetCodeDomain cardsBySetCode,
        ICardsByNameDomain cardsByName,
        ICardNameSearchDomain cardNameSearch)
    {
        _cardsByIds = cardsByIds;
        _cardsBySetCode = cardsBySetCode;
        _cardsByName = cardsByName;
        _cardNameSearch = cardNameSearch;
    }

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByIdsAsync(
        ICardIdsItrEntity args,
        CancellationToken cancellationToken)
        => await _cardsByIds.Execute(args, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsBySetCodeAsync(
        ISetCodeItrEntity setCode,
        CancellationToken cancellationToken)
        => await _cardsBySetCode.Execute(setCode, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByNameAsync(
        ICardNameItrEntity cardName,
        CancellationToken cancellationToken)
        => await _cardsByName.Execute(cardName, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICardNameSearchCollectionOufEntity>> CardNameSearchAsync(
        ICardSearchTermItrEntity searchTerm,
        CancellationToken cancellationToken)
        => await _cardNameSearch.Execute(searchTerm, cancellationToken).ConfigureAwait(false);
}
