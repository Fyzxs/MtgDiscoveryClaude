using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.DataModels.Entities.Oufs.Collections;
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
/// - Input: Currently uses ItrEntity parameters (to be migrated to XfrEntity)
/// - Output: Returns OufEntity types for the aggregator layer
/// - Aggregator layer handles mapping from ItrEntity to XfrEntity
/// </summary>
public interface ICollectionCommandAdapter
{
    /// <summary>
    /// Creates a new collection for a user.
    /// </summary>
    Task<IOperationResponse<ICollectionOufEntity>> CreateCollectionAsync(ICollectionItrEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Renames an existing collection.
    /// </summary>
    Task<IOperationResponse<ICollectionOufEntity>> RenameCollectionAsync(IRenameCollectionItrEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Updates the visibility setting of a collection (public/private).
    /// </summary>
    Task<IOperationResponse<ICollectionOufEntity>> UpdateCollectionVisibilityAsync(IUpdateCollectionVisibilityItrEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Grants a user access to a collection with a specified role.
    /// </summary>
    Task<IOperationResponse<ICollectionOufEntity>> GrantCollectionAccessAsync(IGrantCollectionAccessItrEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Revokes a user's access to a collection.
    /// </summary>
    Task<IOperationResponse<ICollectionOufEntity>> RevokeCollectionAccessAsync(IRevokeCollectionAccessItrEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes a collection. Cannot delete the default collection.
    /// </summary>
    Task<IOperationResponse<ICollectionOufEntity>> DeleteCollectionAsync(IDeleteCollectionItrEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Transfers ownership of a collection to another user.
    /// Cannot transfer the default collection.
    /// </summary>
    Task<IOperationResponse<ICollectionOufEntity>> TransferCollectionOwnershipAsync(ITransferCollectionOwnershipItrEntity entity, CancellationToken cancellationToken);
}
