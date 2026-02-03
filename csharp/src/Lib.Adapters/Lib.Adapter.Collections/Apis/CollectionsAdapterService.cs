using System.Collections.Generic;
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

    public async Task<IOperationResponse<ICollectionOufEntity>> CreateCollectionAsync(ICollectionItrEntity entity) => await _commandAdapter.CreateCollectionAsync(entity).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> RenameCollectionAsync(IRenameCollectionItrEntity entity) => await _commandAdapter.RenameCollectionAsync(entity).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> UpdateCollectionVisibilityAsync(IUpdateCollectionVisibilityItrEntity entity) => await _commandAdapter.UpdateCollectionVisibilityAsync(entity).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> GrantCollectionAccessAsync(IGrantCollectionAccessItrEntity entity) => await _commandAdapter.GrantCollectionAccessAsync(entity).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> RevokeCollectionAccessAsync(IRevokeCollectionAccessItrEntity entity) => await _commandAdapter.RevokeCollectionAccessAsync(entity).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> DeleteCollectionAsync(IDeleteCollectionItrEntity entity) => await _commandAdapter.DeleteCollectionAsync(entity).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> TransferCollectionOwnershipAsync(ITransferCollectionOwnershipItrEntity entity) => await _commandAdapter.TransferCollectionOwnershipAsync(entity).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> GetDefaultCollectionAsync(IOwnerIdItrEntity args) => await _queryAdapter.GetDefaultCollectionAsync(args).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetCollectionsByOwnerAsync(IOwnerIdItrEntity args) => await _queryAdapter.GetCollectionsByOwnerAsync(args).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> GetCollectionByIdAsync(ICollectionIdItrEntity args) => await _queryAdapter.GetCollectionByIdAsync(args).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetSharedCollectionsAsync(IUserIdItrEntity args) => await _queryAdapter.GetSharedCollectionsAsync(args).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetAccessibleCollectionsAsync(IUserIdItrEntity args) => await _queryAdapter.GetAccessibleCollectionsAsync(args).ConfigureAwait(false);
}
