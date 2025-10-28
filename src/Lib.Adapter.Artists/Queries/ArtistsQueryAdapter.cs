using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Artists.Apis;
using Lib.Adapter.Artists.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Artists.Queries;

/// <summary>
/// Cosmos DB implementation of the artist query adapter.
///
/// This class coordinates all Cosmos DB-specific artist query operations
/// by delegating to specialized single-method adapters.
/// The main ArtistAdapterService delegates to this implementation.
/// </summary>
internal sealed class ArtistsQueryAdapter : IArtistQueryAdapter
{
    private readonly ISearchArtistsAdapter _searchArtistsAdapter;
    private readonly ICardsByArtistIdAdapter _cardsByArtistIdAdapter;
    private readonly ICardsByArtistNameAdapter _cardsByArtistNameAdapter;

    public ArtistsQueryAdapter(ILogger logger) : this(
        new SearchArtistsAdapter(logger),
        new CardsByArtistIdAdapter(logger),
        new CardsByArtistNameAdapter(logger))
    {
    }

    private ArtistsQueryAdapter(
        ISearchArtistsAdapter searchArtistsAdapter,
        ICardsByArtistIdAdapter cardsByArtistIdAdapter,
        ICardsByArtistNameAdapter cardsByArtistNameAdapter)
    {
        _searchArtistsAdapter = searchArtistsAdapter;
        _cardsByArtistIdAdapter = cardsByArtistIdAdapter;
        _cardsByArtistNameAdapter = cardsByArtistNameAdapter;
    }

    public Task<IOperationResponse<IEnumerable<ArtistNameTrigramDataExtEntity>>> SearchArtistsAsync(IArtistSearchTermXfrEntity searchTerm) => _searchArtistsAdapter.Execute(searchTerm);

    public Task<IOperationResponse<IEnumerable<ScryfallArtistCardExtEntity>>> CardsByArtistIdAsync(IArtistIdXfrEntity artistId) => _cardsByArtistIdAdapter.Execute(artistId);

    public Task<IOperationResponse<IEnumerable<ScryfallArtistCardExtEntity>>> CardsByArtistNameAsync(IArtistNameXfrEntity artistName) => _cardsByArtistNameAdapter.Execute(artistName);
}
