using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.DataModels.Entities.Itrs.User;
using Lib.Shared.DataModels.Entities.Oufs.Collections;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.Collections.Apis;

/// <summary>
/// Specialized adapter interface for collection query operations.
///
/// This interface represents the query-specific adapter functionality,
/// separate from the main ICollectionsAdapterService which coordinates all adapters.
///
/// Pattern: Main service inherits from specialized interfaces
///   ICollectionsAdapterService : ICollectionQueryAdapter, ICollectionCommandAdapter
///
/// Design Decision: Public specialized interface
/// While concrete implementations are internal, the specialized interfaces are public
/// to allow the main service interface to inherit from them and provide a unified API.
///
/// Entity Mapping Approach:
/// - Input: Currently uses ItrEntity parameters (to be migrated to XfrEntity)
/// - Output: Returns OufEntity types for the aggregator layer
/// - Aggregator layer handles mapping from ItrEntity to XfrEntity
/// </summary>
public interface ICollectionQueryAdapter
{
    /// <summary>
    /// Retrieves the default collection for an owner.
    /// </summary>
    Task<IOperationResponse<ICollectionOufEntity>> GetDefaultCollectionAsync(IOwnerIdItrEntity args);

    /// <summary>
    /// Retrieves all collections owned by a specific user.
    /// </summary>
    Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetCollectionsByOwnerAsync(IOwnerIdItrEntity args);

    /// <summary>
    /// Retrieves a specific collection by its ID.
    /// Supports owner lookup first, then cross-partition query for public collections.
    /// </summary>
    Task<IOperationResponse<ICollectionOufEntity>> GetCollectionByIdAsync(ICollectionIdItrEntity args);

    /// <summary>
    /// Retrieves all collections shared with a specific user (where user is in authorized_users).
    /// </summary>
    Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetSharedCollectionsAsync(IUserIdItrEntity args);

    /// <summary>
    /// Retrieves all collections accessible to a user (owned + shared).
    /// </summary>
    Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetAccessibleCollectionsAsync(IUserIdItrEntity args);
}
