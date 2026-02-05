using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.Collections.Apis;

/// <summary>
/// Specialized adapter interface for collection command operations.
///
/// This interface represents the command-specific adapter functionality,
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
public interface ICollectionCommandAdapter
{
    /// <summary>
    /// Creates a new collection for a user.
    /// </summary>
    Task<IOperationResponse<CollectionExtEntity>> CreateCollectionAsync(ICollectionXfrEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Renames an existing collection.
    /// </summary>
    Task<IOperationResponse<CollectionExtEntity>> RenameCollectionAsync(IRenameCollectionXfrEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Updates the visibility setting of a collection (public/private).
    /// </summary>
    Task<IOperationResponse<CollectionExtEntity>> UpdateCollectionVisibilityAsync(IUpdateCollectionVisibilityXfrEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Grants a user access to a collection with a specified role.
    /// </summary>
    Task<IOperationResponse<CollectionExtEntity>> GrantCollectionAccessAsync(IGrantCollectionAccessXfrEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Revokes a user's access to a collection.
    /// </summary>
    Task<IOperationResponse<CollectionExtEntity>> RevokeCollectionAccessAsync(IRevokeCollectionAccessXfrEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes a collection. Cannot delete the default collection.
    /// </summary>
    Task<IOperationResponse<CollectionExtEntity>> DeleteCollectionAsync(IDeleteCollectionXfrEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Transfers ownership of a collection to another user.
    /// Cannot transfer the default collection.
    /// </summary>
    Task<IOperationResponse<CollectionExtEntity>> TransferCollectionOwnershipAsync(ITransferCollectionOwnershipXfrEntity entity, CancellationToken cancellationToken);
}
