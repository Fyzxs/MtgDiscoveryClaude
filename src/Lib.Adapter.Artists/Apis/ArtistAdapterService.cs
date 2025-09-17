using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Artists.Apis.Entities;
using Lib.Adapter.Artists.Queries;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Artists.Apis;

/// <summary>
/// Main artist adapter service implementation following the passthrough pattern.
/// 
/// This service coordinates all artist-related adapter operations by delegating
/// to specialized adapters. Currently delegates to IArtistQueryAdapter for all
/// operations, but provides the architectural structure for future expansion.
/// 
/// Future Expansion Examples:
///   - IArtistCacheAdapter for caching layer
///   - IArtistFallbackAdapter for redundancy
///   - IArtistMetricsAdapter for telemetry
/// 
/// Pattern Consistency:
/// Matches EntryService, DomainService, and AggregatorService patterns
/// to maintain predictable architecture across all layers.
/// </summary>
public sealed class ArtistAdapterService : IArtistAdapterService
{
    private readonly IArtistQueryAdapter _artistQueryAdapter;

    public ArtistAdapterService(ILogger logger) : this(new ArtistCosmosQueryAdapter(logger))
    { }

    private ArtistAdapterService(IArtistQueryAdapter artistQueryAdapter)
    {
        _artistQueryAdapter = artistQueryAdapter;
    }

    /// <summary>
    /// Searches for artists using trigram-based matching on artist names.
    /// Delegates to the query adapter for implementation.
    /// </summary>
    /// <param name="searchTerm">The search term containing trigrams and normalized form for matching</param>
    /// <returns>Collection of artist trigram data entities that match the search criteria</returns>
    public async Task<IOperationResponse<IEnumerable<ArtistNameTrigramDataExtEntity>>> SearchArtistsAsync(IArtistSearchTermXrfEntity searchTerm)
    {
        return await _artistQueryAdapter.SearchArtistsAsync(searchTerm).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves all cards associated with a specific artist using their unique identifier.
    /// Delegates to the query adapter for implementation.
    /// </summary>
    /// <param name="artistId">The artist identifier transfer entity containing the artist's unique ID</param>
    /// <returns>Collection of Scryfall artist card entities for the specified artist</returns>
    public async Task<IOperationResponse<IEnumerable<ScryfallArtistCardExtEntity>>> CardsByArtistIdAsync(IArtistIdXfrEntity artistId)
    {
        return await _artistQueryAdapter.CardsByArtistIdAsync(artistId).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves all cards associated with an artist using their name with intelligent disambiguation.
    /// Uses trigram-based matching and confidence scoring to resolve artist name ambiguity.
    /// Delegates to the query adapter for implementation.
    /// </summary>
    /// <param name="artistName">The artist name transfer entity containing name, normalized form, and trigrams</param>
    /// <returns>Collection of Scryfall artist card entities for the best-matched artist</returns>
    public async Task<IOperationResponse<IEnumerable<ScryfallArtistCardExtEntity>>> CardsByArtistNameAsync(IArtistNameXfrEntity artistName)
    {
        return await _artistQueryAdapter.CardsByArtistNameAsync(artistName).ConfigureAwait(false);
    }
}
