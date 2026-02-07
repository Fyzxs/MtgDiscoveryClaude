using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
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
/// - Input: Uses XfrEntity parameters following the layered architecture pattern
/// - Output: Returns ExtEntity types (Aggregator handles ExtToOuf mapping)
/// </summary>
public interface ICollectionQueryAdapter
{
    /// <summary>
    /// Retrieves the default collection for an owner.
    /// </summary>
    Task<IOperationResponse<CollectionExtEntity>> GetDefaultCollectionAsync(IOwnerIdXfrEntity args, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all collections owned by a specific user.
    /// </summary>
    Task<IOperationResponse<IEnumerable<CollectionExtEntity>>> GetCollectionsByOwnerAsync(IOwnerIdXfrEntity args, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a specific collection by its ID.
    /// Supports owner lookup first, then cross-partition query for public collections.
    /// </summary>
    Task<IOperationResponse<CollectionExtEntity>> GetCollectionByIdAsync(ICollectionIdXfrEntity args, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all collections accessible to a user (owned + shared).
    /// </summary>
    Task<IOperationResponse<IEnumerable<CollectionExtEntity>>> GetAccessibleCollectionsAsync(IUserIdXfrEntity args, CancellationToken cancellationToken);
}
