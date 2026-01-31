using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Cards.Apis;
using Lib.Adapter.Cards.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Cards.Queries;

/// <summary>
/// Cosmos DB implementation of the card query adapter.
///
/// This class coordinates all Cosmos DB-specific card query operations
/// by delegating to specialized single-method adapters.
/// The main CardAdapterService delegates to this implementation.
/// </summary>
internal sealed class CardsQueryAdapter : ICardQueryAdapter
{
    private readonly ICardsByIdsAdapter _cardsByIdsAdapter;
    private readonly ICardsBySetCodeAdapter _cardsBySetCodeAdapter;
    private readonly ICardsByNameAdapter _cardsByNameAdapter;
    private readonly ISearchCardNamesAdapter _searchCardNamesAdapter;

    public CardsQueryAdapter(ILogger logger) : this(
        new CardsByIdsAdapter(logger),
        new CardsBySetCodeAdapter(logger),
        new CardsByNameAdapter(logger),
        new SearchCardNamesAdapter(logger))
    { }

    private CardsQueryAdapter(
        ICardsByIdsAdapter cardsByIdsAdapter,
        ICardsBySetCodeAdapter cardsBySetCodeAdapter,
        ICardsByNameAdapter cardsByNameAdapter,
        ISearchCardNamesAdapter searchCardNamesAdapter)
    {
        _cardsByIdsAdapter = cardsByIdsAdapter;
        _cardsBySetCodeAdapter = cardsBySetCodeAdapter;
        _cardsByNameAdapter = cardsByNameAdapter;
        _searchCardNamesAdapter = searchCardNamesAdapter;
    }

    public async Task<IOperationResponse<IEnumerable<ScryfallCardItemExtEntity>>> GetCardsByIdsAsync(ICardIdsXfrEntity cardIds) => await _cardsByIdsAdapter.Execute(cardIds);

    public async Task<IOperationResponse<IEnumerable<ScryfallSetCardItemExtEntity>>> GetCardsBySetCodeAsync(ISetCodeXfrEntity setCode) => await _cardsBySetCodeAdapter.Execute(setCode);

    public async Task<IOperationResponse<IEnumerable<ScryfallCardByNameExtEntity>>> GetCardsByNameAsync(ICardNameXfrEntity cardName) => await _cardsByNameAdapter.Execute(cardName);

    public async Task<IOperationResponse<IEnumerable<string>>> SearchCardNamesAsync(ICardSearchTermXfrEntity searchTerm) => await _searchCardNamesAdapter.Execute(searchTerm);
}
