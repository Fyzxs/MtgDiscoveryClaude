using System.Threading.Tasks;
using Lib.Adapter.Artists.Queries;
using Lib.Shared.DataModels.Entities;
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

    public async Task<IOperationResponse<IArtistSearchResultCollectionItrEntity>> SearchArtistsAsync(IArtistSearchTermItrEntity searchTerm)
    {
        return await _artistQueryAdapter.SearchArtistsAsync(searchTerm).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> GetCardsByArtistIdAsync(IArtistIdItrEntity artistId)
    {
        return await _artistQueryAdapter.GetCardsByArtistIdAsync(artistId).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> GetCardsByArtistNameAsync(IArtistNameItrEntity artistName)
    {
        return await _artistQueryAdapter.GetCardsByArtistNameAsync(artistName).ConfigureAwait(false);
    }
}