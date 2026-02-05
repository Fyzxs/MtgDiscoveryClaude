using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Aggregator.Collections.Apis;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.DataModels.Entities.Itrs.User;
using Lib.Shared.DataModels.Entities.Oufs.Collections;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.Collections.Tests.Fakes;

public sealed class CollectionsAggregatorServiceFake : ICollectionsAggregatorService
{
    public IOperationResponse<ICollectionOufEntity> CreateCollectionAsyncResult { get; init; }
    public int CreateCollectionAsyncInvokeCount { get; private set; }

    public IOperationResponse<ICollectionOufEntity> RenameCollectionAsyncResult { get; init; }
    public int RenameCollectionAsyncInvokeCount { get; private set; }

    public IOperationResponse<ICollectionOufEntity> UpdateCollectionVisibilityAsyncResult { get; init; }
    public int UpdateCollectionVisibilityAsyncInvokeCount { get; private set; }

    public IOperationResponse<ICollectionOufEntity> GetDefaultCollectionAsyncResult { get; init; }
    public int GetDefaultCollectionAsyncInvokeCount { get; private set; }

    public IOperationResponse<IEnumerable<ICollectionOufEntity>> GetCollectionsByOwnerAsyncResult { get; init; }
    public int GetCollectionsByOwnerAsyncInvokeCount { get; private set; }

    public IOperationResponse<ICollectionOufEntity> GetCollectionByIdAsyncResult { get; init; }
    public int GetCollectionByIdAsyncInvokeCount { get; private set; }

    public Task<IOperationResponse<ICollectionOufEntity>> CreateCollectionAsync(ICollectionItrEntity entity, CancellationToken cancellationToken)
    {
        CreateCollectionAsyncInvokeCount++;
        return Task.FromResult(CreateCollectionAsyncResult);
    }

    public Task<IOperationResponse<ICollectionOufEntity>> RenameCollectionAsync(IRenameCollectionItrEntity entity, CancellationToken cancellationToken)
    {
        RenameCollectionAsyncInvokeCount++;
        return Task.FromResult(RenameCollectionAsyncResult);
    }

    public Task<IOperationResponse<ICollectionOufEntity>> UpdateCollectionVisibilityAsync(IUpdateCollectionVisibilityItrEntity entity, CancellationToken cancellationToken)
    {
        UpdateCollectionVisibilityAsyncInvokeCount++;
        return Task.FromResult(UpdateCollectionVisibilityAsyncResult);
    }

    public Task<IOperationResponse<ICollectionOufEntity>> GetDefaultCollectionAsync(IOwnerIdItrEntity args, CancellationToken cancellationToken)
    {
        GetDefaultCollectionAsyncInvokeCount++;
        return Task.FromResult(GetDefaultCollectionAsyncResult);
    }

    public Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetCollectionsByOwnerAsync(IOwnerIdItrEntity args, CancellationToken cancellationToken)
    {
        GetCollectionsByOwnerAsyncInvokeCount++;
        return Task.FromResult(GetCollectionsByOwnerAsyncResult);
    }

    public Task<IOperationResponse<ICollectionOufEntity>> GetCollectionByIdAsync(ICollectionIdItrEntity args, CancellationToken cancellationToken)
    {
        GetCollectionByIdAsyncInvokeCount++;
        return Task.FromResult(GetCollectionByIdAsyncResult);
    }

    public IOperationResponse<ICollectionOufEntity> GrantCollectionAccessAsyncResult { get; init; }
    public int GrantCollectionAccessAsyncInvokeCount { get; private set; }

    public Task<IOperationResponse<ICollectionOufEntity>> GrantCollectionAccessAsync(IGrantCollectionAccessItrEntity entity, CancellationToken cancellationToken)
    {
        GrantCollectionAccessAsyncInvokeCount++;
        return Task.FromResult(GrantCollectionAccessAsyncResult);
    }

    public IOperationResponse<ICollectionOufEntity> RevokeCollectionAccessAsyncResult { get; init; }
    public int RevokeCollectionAccessAsyncInvokeCount { get; private set; }

    public Task<IOperationResponse<ICollectionOufEntity>> RevokeCollectionAccessAsync(IRevokeCollectionAccessItrEntity entity, CancellationToken cancellationToken)
    {
        RevokeCollectionAccessAsyncInvokeCount++;
        return Task.FromResult(RevokeCollectionAccessAsyncResult);
    }

    public IOperationResponse<ICollectionOufEntity> DeleteCollectionAsyncResult { get; init; }
    public int DeleteCollectionAsyncInvokeCount { get; private set; }

    public Task<IOperationResponse<ICollectionOufEntity>> DeleteCollectionAsync(IDeleteCollectionItrEntity entity, CancellationToken cancellationToken)
    {
        DeleteCollectionAsyncInvokeCount++;
        return Task.FromResult(DeleteCollectionAsyncResult);
    }

    public IOperationResponse<ICollectionOufEntity> TransferCollectionOwnershipAsyncResult { get; init; }
    public int TransferCollectionOwnershipAsyncInvokeCount { get; private set; }

    public Task<IOperationResponse<ICollectionOufEntity>> TransferCollectionOwnershipAsync(ITransferCollectionOwnershipItrEntity entity, CancellationToken cancellationToken)
    {
        TransferCollectionOwnershipAsyncInvokeCount++;
        return Task.FromResult(TransferCollectionOwnershipAsyncResult);
    }

    public IOperationResponse<IEnumerable<ICollectionOufEntity>> GetSharedCollectionsAsyncResult { get; init; }
    public int GetSharedCollectionsAsyncInvokeCount { get; private set; }

    public Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetSharedCollectionsAsync(IUserIdItrEntity args, CancellationToken cancellationToken)
    {
        GetSharedCollectionsAsyncInvokeCount++;
        return Task.FromResult(GetSharedCollectionsAsyncResult);
    }

    public IOperationResponse<IEnumerable<ICollectionOufEntity>> GetAccessibleCollectionsAsyncResult { get; init; }
    public int GetAccessibleCollectionsAsyncInvokeCount { get; private set; }

    public Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetAccessibleCollectionsAsync(IUserIdItrEntity args, CancellationToken cancellationToken)
    {
        GetAccessibleCollectionsAsyncInvokeCount++;
        return Task.FromResult(GetAccessibleCollectionsAsyncResult);
    }
}
