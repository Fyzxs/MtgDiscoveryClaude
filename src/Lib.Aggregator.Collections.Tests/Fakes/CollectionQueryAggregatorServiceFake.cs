using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Aggregator.Collections.Apis;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.DataModels.Entities.Itrs.User;
using Lib.Shared.DataModels.Entities.Oufs.Collections;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.Collections.Tests.Fakes;

internal sealed class CollectionQueryAggregatorServiceFake : ICollectionQueryAggregatorService
{
    public IOperationResponse<ICollectionOufEntity> GetDefaultCollectionAsyncResult { get; init; }
    public int GetDefaultCollectionAsyncInvokeCount { get; private set; }

    public IOperationResponse<IEnumerable<ICollectionOufEntity>> GetCollectionsByOwnerAsyncResult { get; init; }
    public int GetCollectionsByOwnerAsyncInvokeCount { get; private set; }

    public IOperationResponse<ICollectionOufEntity> GetCollectionByIdAsyncResult { get; init; }
    public int GetCollectionByIdAsyncInvokeCount { get; private set; }

    public IOperationResponse<IEnumerable<ICollectionOufEntity>> GetSharedCollectionsAsyncResult { get; init; }
    public int GetSharedCollectionsAsyncInvokeCount { get; private set; }

    public IOperationResponse<IEnumerable<ICollectionOufEntity>> GetAccessibleCollectionsAsyncResult { get; init; }
    public int GetAccessibleCollectionsAsyncInvokeCount { get; private set; }

    public Task<IOperationResponse<ICollectionOufEntity>> GetDefaultCollectionAsync(IOwnerIdItrEntity args)
    {
        GetDefaultCollectionAsyncInvokeCount++;
        return Task.FromResult(GetDefaultCollectionAsyncResult);
    }

    public Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetCollectionsByOwnerAsync(IOwnerIdItrEntity args)
    {
        GetCollectionsByOwnerAsyncInvokeCount++;
        return Task.FromResult(GetCollectionsByOwnerAsyncResult);
    }

    public Task<IOperationResponse<ICollectionOufEntity>> GetCollectionByIdAsync(ICollectionIdItrEntity args)
    {
        GetCollectionByIdAsyncInvokeCount++;
        return Task.FromResult(GetCollectionByIdAsyncResult);
    }

    public Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetSharedCollectionsAsync(IUserIdItrEntity args)
    {
        GetSharedCollectionsAsyncInvokeCount++;
        return Task.FromResult(GetSharedCollectionsAsyncResult);
    }

    public Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetAccessibleCollectionsAsync(IUserIdItrEntity args)
    {
        GetAccessibleCollectionsAsyncInvokeCount++;
        return Task.FromResult(GetAccessibleCollectionsAsyncResult);
    }
}
