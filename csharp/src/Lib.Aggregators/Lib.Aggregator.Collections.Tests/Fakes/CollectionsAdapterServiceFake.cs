using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.Collections.Tests.Fakes;

public sealed class CollectionsAdapterServiceFake : ICollectionsAdapterService
{
    public IOperationResponse<CollectionExtEntity> CreateCollectionAsyncResult { get; init; }
    public int CreateCollectionAsyncInvokeCount { get; private set; }

    public IOperationResponse<CollectionExtEntity> RenameCollectionAsyncResult { get; init; }
    public int RenameCollectionAsyncInvokeCount { get; private set; }

    public IOperationResponse<CollectionExtEntity> UpdateCollectionVisibilityAsyncResult { get; init; }
    public int UpdateCollectionVisibilityAsyncInvokeCount { get; private set; }

    public IOperationResponse<CollectionExtEntity> GetDefaultCollectionAsyncResult { get; init; }
    public int GetDefaultCollectionAsyncInvokeCount { get; private set; }

    public IOperationResponse<IEnumerable<CollectionExtEntity>> GetCollectionsByOwnerAsyncResult { get; init; }
    public int GetCollectionsByOwnerAsyncInvokeCount { get; private set; }

    public IOperationResponse<CollectionExtEntity> GetCollectionByIdAsyncResult { get; init; }
    public int GetCollectionByIdAsyncInvokeCount { get; private set; }

    public IOperationResponse<CollectionExtEntity> GrantCollectionAccessAsyncResult { get; init; }
    public int GrantCollectionAccessAsyncInvokeCount { get; private set; }

    public IOperationResponse<CollectionExtEntity> RevokeCollectionAccessAsyncResult { get; init; }
    public int RevokeCollectionAccessAsyncInvokeCount { get; private set; }

    public IOperationResponse<CollectionExtEntity> DeleteCollectionAsyncResult { get; init; }
    public int DeleteCollectionAsyncInvokeCount { get; private set; }

    public IOperationResponse<CollectionExtEntity> TransferCollectionOwnershipAsyncResult { get; init; }
    public int TransferCollectionOwnershipAsyncInvokeCount { get; private set; }

    public IOperationResponse<IEnumerable<CollectionExtEntity>> GetAccessibleCollectionsAsyncResult { get; init; }
    public int GetAccessibleCollectionsAsyncInvokeCount { get; private set; }

    public Task<IOperationResponse<CollectionExtEntity>> CreateCollectionAsync(ICollectionXfrEntity entity, CancellationToken cancellationToken)
    {
        CreateCollectionAsyncInvokeCount++;
        return Task.FromResult(CreateCollectionAsyncResult);
    }

    public Task<IOperationResponse<CollectionExtEntity>> RenameCollectionAsync(IRenameCollectionXfrEntity entity, CancellationToken cancellationToken)
    {
        RenameCollectionAsyncInvokeCount++;
        return Task.FromResult(RenameCollectionAsyncResult);
    }

    public Task<IOperationResponse<CollectionExtEntity>> UpdateCollectionVisibilityAsync(IUpdateCollectionVisibilityXfrEntity entity, CancellationToken cancellationToken)
    {
        UpdateCollectionVisibilityAsyncInvokeCount++;
        return Task.FromResult(UpdateCollectionVisibilityAsyncResult);
    }

    public Task<IOperationResponse<CollectionExtEntity>> GetDefaultCollectionAsync(IOwnerIdXfrEntity args, CancellationToken cancellationToken)
    {
        GetDefaultCollectionAsyncInvokeCount++;
        return Task.FromResult(GetDefaultCollectionAsyncResult);
    }

    public Task<IOperationResponse<IEnumerable<CollectionExtEntity>>> GetCollectionsByOwnerAsync(IOwnerIdXfrEntity args, CancellationToken cancellationToken)
    {
        GetCollectionsByOwnerAsyncInvokeCount++;
        return Task.FromResult(GetCollectionsByOwnerAsyncResult);
    }

    public Task<IOperationResponse<CollectionExtEntity>> GetCollectionByIdAsync(ICollectionIdXfrEntity args, CancellationToken cancellationToken)
    {
        GetCollectionByIdAsyncInvokeCount++;
        return Task.FromResult(GetCollectionByIdAsyncResult);
    }

    public Task<IOperationResponse<CollectionExtEntity>> GrantCollectionAccessAsync(IGrantCollectionAccessXfrEntity entity, CancellationToken cancellationToken)
    {
        GrantCollectionAccessAsyncInvokeCount++;
        return Task.FromResult(GrantCollectionAccessAsyncResult);
    }

    public Task<IOperationResponse<CollectionExtEntity>> RevokeCollectionAccessAsync(IRevokeCollectionAccessXfrEntity entity, CancellationToken cancellationToken)
    {
        RevokeCollectionAccessAsyncInvokeCount++;
        return Task.FromResult(RevokeCollectionAccessAsyncResult);
    }

    public Task<IOperationResponse<CollectionExtEntity>> DeleteCollectionAsync(IDeleteCollectionXfrEntity entity, CancellationToken cancellationToken)
    {
        DeleteCollectionAsyncInvokeCount++;
        return Task.FromResult(DeleteCollectionAsyncResult);
    }

    public Task<IOperationResponse<CollectionExtEntity>> TransferCollectionOwnershipAsync(ITransferCollectionOwnershipXfrEntity entity, CancellationToken cancellationToken)
    {
        TransferCollectionOwnershipAsyncInvokeCount++;
        return Task.FromResult(TransferCollectionOwnershipAsyncResult);
    }

    public Task<IOperationResponse<IEnumerable<CollectionExtEntity>>> GetAccessibleCollectionsAsync(IUserIdXfrEntity args, CancellationToken cancellationToken)
    {
        GetAccessibleCollectionsAsyncInvokeCount++;
        return Task.FromResult(GetAccessibleCollectionsAsyncResult);
    }
}
