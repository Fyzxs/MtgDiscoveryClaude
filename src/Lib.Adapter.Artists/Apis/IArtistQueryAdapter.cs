using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Artists.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.Artists.Apis;

/// <summary>
/// Specialized adapter interface for artist query operations.
///
/// This interface represents the query-specific adapter functionality,
/// separate from the main IArtistAdapterService which coordinates all adapters.
///
/// Pattern: Main service inherits from specialized interfaces
///   IArtistAdapterService : IArtistQueryAdapter, IArtistCacheAdapter, IArtistMetricsAdapter
///
/// Design Decision: Public specialized interface
/// While concrete implementations are internal, the specialized interfaces are public
/// to allow the main service interface to inherit from them and provide a unified API.
///
/// Entity Mapping Approach:
/// - Input: Preserves ItrEntity parameters following MicroObjects principles
/// - Output: Returns ExtEntity types directly from storage layer
/// - Aggregator mapping: Aggregator layer maps ExtEntity to ItrEntity
/// Primitive extraction happens in the concrete implementation when interfacing with external systems.
/// </summary>
public interface IArtistQueryAdapter
{
    /// <summary>
    /// Searches for artists using trigram-based matching on artist names.
    /// </summary>
    /// <param name="searchTerm">The search term containing trigrams and normalized form for matching</param>
    /// <returns>Collection of artist trigram data entities that match the search criteria</returns>
    Task<IOperationResponse<IEnumerable<ArtistNameTrigramDataExtEntity>>> SearchArtistsAsync(IArtistSearchTermXfrEntity searchTerm);

    /// <summary>
    /// Retrieves all cards associated with a specific artist using their unique identifier.
    /// </summary>
    /// <param name="artistId">The artist identifier transfer entity containing the artist's unique ID</param>
    /// <returns>Collection of Scryfall artist card entities for the specified artist</returns>
    Task<IOperationResponse<IEnumerable<ScryfallArtistCardExtEntity>>> CardsByArtistIdAsync(IArtistIdXfrEntity artistId);

    /// <summary>
    /// Retrieves all cards associated with an artist using their name with intelligent disambiguation.
    /// Uses trigram-based matching and confidence scoring to resolve artist name ambiguity.
    /// </summary>
    /// <param name="artistName">The artist name transfer entity containing name, normalized form, and trigrams</param>
    /// <returns>Collection of Scryfall artist card entities for the best-matched artist</returns>
    Task<IOperationResponse<IEnumerable<ScryfallArtistCardExtEntity>>> CardsByArtistNameAsync(IArtistNameXfrEntity artistName);
}
