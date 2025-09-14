namespace Lib.Adapter.UserCards.Apis;

/// <summary>
/// Main adapter service interface for user card collection operations.
///
/// Design Decision: Composite interface pattern for unified service access
/// This interface inherits from all specialized adapter interfaces, providing a single
/// public contract that encompasses all UserCards adapter functionality. This maintains
/// architectural consistency across all layers and enables clean future expansion.
///
/// Follows the same pattern as:
///   - IEntryService -> IQueryEntryService, ICacheEntryService, etc.
///   - IDomainService -> IQueryDomainService, IValidationDomainService, etc.
///   - IAggregatorService -> IQueryAggregatorService, ICacheAggregatorService, etc.
///
/// Future Expansion Pattern:
/// When new specialized adapters are added, they are inherited here:
/// public interface IUserCardsAdapterService : IUserCardsCommandAdapter, IUserCardsCacheAdapter, IUserCardsMetricsAdapter
///
/// Entity Mapping Approach:
/// This interface accepts complete ItrEntity objects instead of extracting primitives
/// at the layer boundary, following MicroObjects principles. The adapter implementation
/// handles internal primitive extraction when needed for external system calls.
/// </summary>
public interface IUserCardsAdapterService : IUserCardsCommandAdapter
{
    // All method signatures inherited from IUserCardsCommandAdapter
    // Future specialized interfaces (IUserCardsCacheAdapter, IUserCardsMetricsAdapter, etc.)
    // will be added to the inheritance list above
}