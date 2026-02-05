using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Commands;
using Lib.Adapter.Collections.Queries;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.DataModels.Entities.Itrs.User;
using Lib.Shared.DataModels.Entities.Oufs.Collections;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Collections.Apis;

/// <summary>
/// Composite service that coordinates all collection adapter operations.
/// Implements the passthrough pattern, delegating to specialized query and command adapters.
///
/// Architecture: AdapterService → QueryAdapter/CommandAdapter → Single-Operation Adapters
///
/// This service provides a unified API for all collection operations while internally
/// organizing the implementation across specialized adapters for queries and commands.
/// </summary>
public sealed class CollectionsAdapterService : ICollectionsAdapterService
{
    private readonly ICollectionCommandAdapter _commandAdapter;
    private readonly ICollectionQueryAdapter _queryAdapter;

    public CollectionsAdapterService(ILogger logger) : this(
        new CollectionCommandAdapter(logger),
        new CollectionQueryAdapter(logger))
    { }

    private CollectionsAdapterService(
        ICollectionCommandAdapter commandAdapter,
        ICollectionQueryAdapter queryAdapter)
    {
        _commandAdapter = commandAdapter;
        _queryAdapter = queryAdapter;
    }

    public async Task<IOperationResponse<ICollectionOufEntity>> CreateCollectionAsync(ICollectionItrEntity entity, CancellationToken cancellationToken) => await _commandAdapter.CreateCollectionAsync(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> RenameCollectionAsync(IRenameCollectionItrEntity entity, CancellationToken cancellationToken) => await _commandAdapter.RenameCollectionAsync(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> UpdateCollectionVisibilityAsync(IUpdateCollectionVisibilityItrEntity entity, CancellationToken cancellationToken) => await _commandAdapter.UpdateCollectionVisibilityAsync(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> GrantCollectionAccessAsync(IGrantCollectionAccessItrEntity entity, CancellationToken cancellationToken) => await _commandAdapter.GrantCollectionAccessAsync(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> RevokeCollectionAccessAsync(IRevokeCollectionAccessItrEntity entity, CancellationToken cancellationToken) => await _commandAdapter.RevokeCollectionAccessAsync(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> DeleteCollectionAsync(IDeleteCollectionItrEntity entity, CancellationToken cancellationToken) => await _commandAdapter.DeleteCollectionAsync(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> TransferCollectionOwnershipAsync(ITransferCollectionOwnershipItrEntity entity, CancellationToken cancellationToken) => await _commandAdapter.TransferCollectionOwnershipAsync(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> GetDefaultCollectionAsync(IOwnerIdItrEntity args, CancellationToken cancellationToken) => await _queryAdapter.GetDefaultCollectionAsync(args, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetCollectionsByOwnerAsync(IOwnerIdItrEntity args, CancellationToken cancellationToken) => await _queryAdapter.GetCollectionsByOwnerAsync(args, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> GetCollectionByIdAsync(ICollectionIdItrEntity args, CancellationToken cancellationToken) => await _queryAdapter.GetCollectionByIdAsync(args, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetSharedCollectionsAsync(IUserIdItrEntity args, CancellationToken cancellationToken) => await _queryAdapter.GetSharedCollectionsAsync(args, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetAccessibleCollectionsAsync(IUserIdItrEntity args, CancellationToken cancellationToken) => await _queryAdapter.GetAccessibleCollectionsAsync(args, cancellationToken).ConfigureAwait(false);
}
