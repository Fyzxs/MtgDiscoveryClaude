namespace Lib.Adapter.User.Apis;

/// <summary>
/// Main adapter service interface for user operations.
///
/// Design Decision: Composite interface pattern for unified service access
/// This interface inherits from all specialized adapter interfaces, providing a single
/// public contract that encompasses all user adapter functionality. This maintains
/// architectural consistency across all layers and enables clean future expansion.
///
/// Follows the same pattern as:
///   - IEntryService -> IQueryEntryService, ICacheEntryService, etc.
///   - IDomainService -> IQueryDomainService, IValidationDomainService, etc.
///   - IAggregatorService -> IQueryAggregatorService, ICacheAggregatorService, etc.
///
/// Future Expansion Pattern:
/// When new specialized adapters are added, they are inherited here:
/// public interface IUserAdapterService : IUserPersistenceAdapter, IUserCacheAdapter, IUserMetricsAdapter
///
/// Entity Mapping Approach:
/// - Input: IXfrEntity (transfer entity from aggregator)
/// - Output: ExtEntity (external representation)
/// Aggregator handles ItrEntity → XfrEntity and ExtEntity → OufEntity mappings.
/// </summary>
public interface IUserAdapterService : IUserCommandAdapter
{
    // All method signatures inherited from IUserCommandAdapter
    // Future specialized interfaces (IUserCacheAdapter, IUserMetricsAdapter, etc.)
    // will be added to the inheritance list above
}
