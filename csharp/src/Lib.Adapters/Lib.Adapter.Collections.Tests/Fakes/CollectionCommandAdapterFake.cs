using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.Collections.Tests.Fakes;

public sealed class CollectionCommandAdapterFake : ICollectionCommandAdapter
{
    public IOperationResponse<CollectionExtEntity> CreateCollectionAsyncResult { get; init; }
    public int CreateCollectionAsyncInvokeCount { get; private set; }

    public IOperationResponse<CollectionExtEntity> RenameCollectionAsyncResult { get; init; }
    public int RenameCollectionAsyncInvokeCount { get; private set; }

    public IOperationResponse<CollectionExtEntity> UpdateCollectionVisibilityAsyncResult { get; init; }
    public int UpdateCollectionVisibilityAsyncInvokeCount { get; private set; }

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

    public IOperationResponse<CollectionExtEntity> GrantCollectionAccessAsyncResult { get; init; }
    public int GrantCollectionAccessAsyncInvokeCount { get; private set; }

    public Task<IOperationResponse<CollectionExtEntity>> GrantCollectionAccessAsync(IGrantCollectionAccessXfrEntity entity, CancellationToken cancellationToken)
    {
        GrantCollectionAccessAsyncInvokeCount++;
        return Task.FromResult(GrantCollectionAccessAsyncResult);
    }

    public IOperationResponse<CollectionExtEntity> RevokeCollectionAccessAsyncResult { get; init; }
    public int RevokeCollectionAccessAsyncInvokeCount { get; private set; }

    public Task<IOperationResponse<CollectionExtEntity>> RevokeCollectionAccessAsync(IRevokeCollectionAccessXfrEntity entity, CancellationToken cancellationToken)
    {
        RevokeCollectionAccessAsyncInvokeCount++;
        return Task.FromResult(RevokeCollectionAccessAsyncResult);
    }

    public IOperationResponse<CollectionExtEntity> DeleteCollectionAsyncResult { get; init; }
    public int DeleteCollectionAsyncInvokeCount { get; private set; }

    public Task<IOperationResponse<CollectionExtEntity>> DeleteCollectionAsync(IDeleteCollectionXfrEntity entity, CancellationToken cancellationToken)
    {
        DeleteCollectionAsyncInvokeCount++;
        return Task.FromResult(DeleteCollectionAsyncResult);
    }

    public IOperationResponse<CollectionExtEntity> TransferCollectionOwnershipAsyncResult { get; init; }
    public int TransferCollectionOwnershipAsyncInvokeCount { get; private set; }

    public Task<IOperationResponse<CollectionExtEntity>> TransferCollectionOwnershipAsync(ITransferCollectionOwnershipXfrEntity entity, CancellationToken cancellationToken)
    {
        TransferCollectionOwnershipAsyncInvokeCount++;
        return Task.FromResult(TransferCollectionOwnershipAsyncResult);
    }
}
