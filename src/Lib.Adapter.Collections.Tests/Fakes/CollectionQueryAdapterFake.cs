using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis;
using Lib.Shared.DataModels.Entities.Oufs.Collections;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.Collections.Tests.Fakes;

internal sealed class CollectionQueryAdapterFake : ICollectionQueryAdapter
{
    public IOperationResponse<ICollectionOufEntity> GetDefaultCollectionAsyncResult { get; init; }
    public int GetDefaultCollectionAsyncInvokeCount { get; private set; }

    public IOperationResponse<IEnumerable<ICollectionOufEntity>> GetCollectionsByOwnerAsyncResult { get; init; }
    public int GetCollectionsByOwnerAsyncInvokeCount { get; private set; }

    public IOperationResponse<ICollectionOufEntity> GetCollectionByIdAsyncResult { get; init; }
    public int GetCollectionByIdAsyncInvokeCount { get; private set; }

    public Task<IOperationResponse<ICollectionOufEntity>> GetDefaultCollectionAsync(string ownerId)
    {
        GetDefaultCollectionAsyncInvokeCount++;
        return Task.FromResult(GetDefaultCollectionAsyncResult);
    }

    public Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetCollectionsByOwnerAsync(string ownerId)
    {
        GetCollectionsByOwnerAsyncInvokeCount++;
        return Task.FromResult(GetCollectionsByOwnerAsyncResult);
    }

    public Task<IOperationResponse<ICollectionOufEntity>> GetCollectionByIdAsync(string collectionId, string ownerId)
    {
        GetCollectionByIdAsyncInvokeCount++;
        return Task.FromResult(GetCollectionByIdAsyncResult);
    }

    public IOperationResponse<IEnumerable<ICollectionOufEntity>> GetSharedCollectionsAsyncResult { get; init; }
    public int GetSharedCollectionsAsyncInvokeCount { get; private set; }

    public Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetSharedCollectionsAsync(string userId)
    {
        GetSharedCollectionsAsyncInvokeCount++;
        return Task.FromResult(GetSharedCollectionsAsyncResult);
    }

    public IOperationResponse<IEnumerable<ICollectionOufEntity>> GetAccessibleCollectionsAsyncResult { get; init; }
    public int GetAccessibleCollectionsAsyncInvokeCount { get; private set; }

    public Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetAccessibleCollectionsAsync(string userId)
    {
        GetAccessibleCollectionsAsyncInvokeCount++;
        return Task.FromResult(GetAccessibleCollectionsAsyncResult);
    }
}
