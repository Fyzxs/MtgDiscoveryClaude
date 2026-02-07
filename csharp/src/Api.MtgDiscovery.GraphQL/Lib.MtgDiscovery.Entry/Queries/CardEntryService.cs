using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.MtgDiscovery.Entry.Queries.Cards;
using Lib.Shared.DataModels.Entities.Args.Cards;
using Lib.Shared.DataModels.Entities.Args.Sets;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries;

internal sealed class CardEntryService : ICardEntryService
{
    private readonly ICardsByIdsEntryService _cardsByIds;
    private readonly ICardsBySetCodeEntryService _cardsBySetCode;
    private readonly ICardsByNameEntryService _cardsByName;
    private readonly ICardNameSearchEntryService _cardNameSearch;

    public CardEntryService(ILogger logger) : this(
        new CardsByIdsEntryService(logger),
        new CardsBySetCodeEntryService(logger),
        new CardsByNameEntryService(logger),
        new CardNameSearchEntryService(logger))
    { }

    private CardEntryService(
        ICardsByIdsEntryService cardsByIds,
        ICardsBySetCodeEntryService cardsBySetCode,
        ICardsByNameEntryService cardsByName,
        ICardNameSearchEntryService cardNameSearch)
    {
        _cardsByIds = cardsByIds;
        _cardsBySetCode = cardsBySetCode;
        _cardsByName = cardsByName;
        _cardNameSearch = cardNameSearch;
    }

    public async Task<IOperationResponse<List<CardItemOutEntity>>> CardsByIdsAsync(
        ICardIdsArgEntity args,
        CancellationToken cancellationToken)
        => await _cardsByIds.Execute(args, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<List<CardItemOutEntity>>> CardsBySetCodeAsync(
        ISetCodeArgEntity setCode,
        CancellationToken cancellationToken)
        => await _cardsBySetCode.Execute(setCode, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<List<CardItemOutEntity>>> CardsByNameAsync(
        ICardNameArgEntity cardName,
        CancellationToken cancellationToken)
        => await _cardsByName.Execute(cardName, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<List<CardNameSearchResultOutEntity>>> CardNameSearchAsync(
        ICardSearchTermArgEntity searchTerm,
        CancellationToken cancellationToken)
        => await _cardNameSearch.Execute(searchTerm, cancellationToken).ConfigureAwait(false);
}
