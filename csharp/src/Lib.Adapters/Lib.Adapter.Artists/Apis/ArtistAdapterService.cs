using System.Collections.Generic;
using System.Threading;
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

    public ArtistAdapterService(ILogger logger) : this(new ArtistsQueryAdapter(logger))
    { }

    private ArtistAdapterService(IArtistQueryAdapter artistQueryAdapter) => _artistQueryAdapter = artistQueryAdapter;

    /// <summary>
    /// Searches for artists using trigram-based matching on artist names.
    /// Delegates to the query adapter for implementation.
    /// </summary>
    public async Task<IOperationResponse<IEnumerable<ArtistNameTrigramDataExtEntity>>> SearchArtistsAsync(
        IArtistSearchTermXfrEntity searchTerm,
        CancellationToken cancellationToken) => await _artistQueryAdapter.SearchArtistsAsync(searchTerm, cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Retrieves all cards associated with a specific artist using their unique identifier.
    /// Delegates to the query adapter for implementation.
    /// </summary>
    public async Task<IOperationResponse<IEnumerable<ScryfallArtistCardExtEntity>>> CardsByArtistIdAsync(
        IArtistIdXfrEntity artistId,
        CancellationToken cancellationToken) => await _artistQueryAdapter.CardsByArtistIdAsync(artistId, cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Retrieves all cards associated with an artist using their name with intelligent disambiguation.
    /// Uses trigram-based matching and confidence scoring to resolve artist name ambiguity.
    /// Delegates to the query adapter for implementation.
    /// </summary>
    public async Task<IOperationResponse<IEnumerable<ScryfallArtistCardExtEntity>>> CardsByArtistNameAsync(
        IArtistNameXfrEntity artistName,
        CancellationToken cancellationToken) => await _artistQueryAdapter.CardsByArtistNameAsync(artistName, cancellationToken).ConfigureAwait(false);
}
