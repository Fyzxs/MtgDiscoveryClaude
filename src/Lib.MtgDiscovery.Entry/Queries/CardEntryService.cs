using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Queries.Cards;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Outs.Cards;
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

    public async Task<IOperationResponse<List<CardItemOutEntity>>> CardsByIdsAsync(ICardIdsArgEntity args) => await _cardsByIds.Execute(args).ConfigureAwait(false);

    public async Task<IOperationResponse<List<CardItemOutEntity>>> CardsBySetCodeAsync(ISetCodeArgEntity setCode) => await _cardsBySetCode.Execute(setCode).ConfigureAwait(false);

    public async Task<IOperationResponse<List<CardItemOutEntity>>> CardsByNameAsync(ICardNameArgEntity cardName) => await _cardsByName.Execute(cardName).ConfigureAwait(false);

    public async Task<IOperationResponse<List<CardNameSearchResultOutEntity>>> CardNameSearchAsync(ICardSearchTermArgEntity searchTerm) => await _cardNameSearch.Execute(searchTerm).ConfigureAwait(false);
}
