namespace Lib.Adapter.Sets.Apis;

/// <summary>
/// Main adapter service interface for set operations.
/// 
/// Design Decision: Composite interface pattern for unified service access
/// This interface inherits from all specialized adapter interfaces, providing a single
/// public contract that encompasses all set adapter functionality. This maintains
/// architectural consistency across all layers and enables clean future expansion.
/// 
/// Follows the same pattern as:
///   - IEntryService -> IQueryEntryService, ICacheEntryService, etc.
///   - IDomainService -> IQueryDomainService, IValidationDomainService, etc.
///   - IAggregatorService -> IQueryAggregatorService, ICacheAggregatorService, etc.
/// 
/// Future Expansion Pattern:
/// When new specialized adapters are added, they are inherited here:
/// public interface ISetAdapterService : ISetQueryAdapter, ISetCacheAdapter, ISetMetricsAdapter
/// 
/// Entity Mapping Approach:
/// This interface accepts complete ItrEntity objects instead of extracting primitives
/// at the layer boundary, following MicroObjects principles. The adapter implementation
/// handles internal primitive extraction when needed for external system calls.
/// </summary>
public interface ISetAdapterService : ISetQueryAdapter
{
    // All method signatures inherited from ISetQueryAdapter
    // Future specialized interfaces (ISetCacheAdapter, ISetMetricsAdapter, etc.) 
    // will be added to the inheritance list above
}
