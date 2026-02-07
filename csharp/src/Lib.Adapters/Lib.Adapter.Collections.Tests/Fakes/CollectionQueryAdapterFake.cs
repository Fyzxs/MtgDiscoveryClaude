using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.Collections.Tests.Fakes;

public sealed class CollectionQueryAdapterFake : ICollectionQueryAdapter
{
    public IOperationResponse<CollectionExtEntity> GetDefaultCollectionAsyncResult { get; init; }
    public int GetDefaultCollectionAsyncInvokeCount { get; private set; }

    public IOperationResponse<IEnumerable<CollectionExtEntity>> GetCollectionsByOwnerAsyncResult { get; init; }
    public int GetCollectionsByOwnerAsyncInvokeCount { get; private set; }

    public IOperationResponse<CollectionExtEntity> GetCollectionByIdAsyncResult { get; init; }
    public int GetCollectionByIdAsyncInvokeCount { get; private set; }

    public IOperationResponse<IEnumerable<CollectionExtEntity>> GetAccessibleCollectionsAsyncResult { get; init; }
    public int GetAccessibleCollectionsAsyncInvokeCount { get; private set; }

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

    public Task<IOperationResponse<IEnumerable<CollectionExtEntity>>> GetAccessibleCollectionsAsync(IUserIdXfrEntity args, CancellationToken cancellationToken)
    {
        GetAccessibleCollectionsAsyncInvokeCount++;
        return Task.FromResult(GetAccessibleCollectionsAsyncResult);
    }
}
