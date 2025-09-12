using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities;
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
/// - Output: Returns ITR entities for consistency with main service interface
/// - Internal mapping: Adapter implementations map from storage entities to ITR entities
/// Primitive extraction happens in the concrete implementation when interfacing with external systems.
/// </summary>
public interface IArtistQueryAdapter
{
    Task<IOperationResponse<IArtistSearchResultCollectionItrEntity>> SearchArtistsAsync(IArtistSearchTermItrEntity searchTerm);
    Task<IOperationResponse<ICardItemCollectionItrEntity>> GetCardsByArtistIdAsync(IArtistIdItrEntity artistId);
    Task<IOperationResponse<ICardItemCollectionItrEntity>> GetCardsByArtistNameAsync(IArtistNameItrEntity artistName);
}
