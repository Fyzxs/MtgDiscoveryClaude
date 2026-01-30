using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Commands;
using Lib.Adapter.Collections.Queries;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.DataModels.Entities.Oufs.Collections;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Collections.Apis;

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

    public Task<IOperationResponse<ICollectionOufEntity>> CreateCollectionAsync(ICollectionItrEntity entity) =>
        _commandAdapter.CreateCollectionAsync(entity);

    public Task<IOperationResponse<ICollectionOufEntity>> RenameCollectionAsync(IRenameCollectionItrEntity entity) =>
        _commandAdapter.RenameCollectionAsync(entity);

    public Task<IOperationResponse<ICollectionOufEntity>> UpdateCollectionVisibilityAsync(IUpdateCollectionVisibilityItrEntity entity) =>
        _commandAdapter.UpdateCollectionVisibilityAsync(entity);

    public Task<IOperationResponse<ICollectionOufEntity>> GrantCollectionAccessAsync(IGrantCollectionAccessItrEntity entity) =>
        _commandAdapter.GrantCollectionAccessAsync(entity);

    public Task<IOperationResponse<ICollectionOufEntity>> RevokeCollectionAccessAsync(IRevokeCollectionAccessItrEntity entity) =>
        _commandAdapter.RevokeCollectionAccessAsync(entity);

    public Task<IOperationResponse<ICollectionOufEntity>> DeleteCollectionAsync(IDeleteCollectionItrEntity entity) =>
        _commandAdapter.DeleteCollectionAsync(entity);

    public Task<IOperationResponse<ICollectionOufEntity>> TransferCollectionOwnershipAsync(ITransferCollectionOwnershipItrEntity entity) =>
        _commandAdapter.TransferCollectionOwnershipAsync(entity);

    public Task<IOperationResponse<ICollectionOufEntity>> GetDefaultCollectionAsync(string ownerId) =>
        _queryAdapter.GetDefaultCollectionAsync(ownerId);

    public Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetCollectionsByOwnerAsync(string ownerId) =>
        _queryAdapter.GetCollectionsByOwnerAsync(ownerId);

    public Task<IOperationResponse<ICollectionOufEntity>> GetCollectionByIdAsync(string collectionId, string ownerId) =>
        _queryAdapter.GetCollectionByIdAsync(collectionId, ownerId);

    public Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetSharedCollectionsAsync(string userId) =>
        _queryAdapter.GetSharedCollectionsAsync(userId);

    public Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetAccessibleCollectionsAsync(string userId) =>
        _queryAdapter.GetAccessibleCollectionsAsync(userId);
}
