using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Domain.Collections.Apis;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.DataModels.Entities.Itrs.User;
using Lib.Shared.DataModels.Entities.Oufs.Collections;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Tests.Commands.Collections.Fakes;

internal sealed class CollectionsDomainServiceFake : ICollectionsDomainService
{
    public IOperationResponse<ICollectionOufEntity> CreateCollectionAsyncResult { get; init; }
    public int CreateCollectionAsyncInvokeCount { get; private set; }
    public ICollectionItrEntity CreateCollectionAsyncLastEntity { get; private set; }

    public IOperationResponse<ICollectionOufEntity> GetDefaultCollectionAsyncResult { get; init; }
    public int GetDefaultCollectionAsyncInvokeCount { get; private set; }
    public IOwnerIdItrEntity GetDefaultCollectionAsyncLastArgs { get; private set; }

    public IOperationResponse<IEnumerable<ICollectionOufEntity>> GetCollectionsByOwnerAsyncResult { get; init; }
    public int GetCollectionsByOwnerAsyncInvokeCount { get; private set; }
    public IOwnerIdItrEntity GetCollectionsByOwnerAsyncLastArgs { get; private set; }

    public IOperationResponse<ICollectionOufEntity> GetCollectionByIdAsyncResult { get; init; }
    public int GetCollectionByIdAsyncInvokeCount { get; private set; }
    public ICollectionIdItrEntity GetCollectionByIdAsyncLastArgs { get; private set; }

    public IOperationResponse<IEnumerable<ICollectionOufEntity>> GetSharedCollectionsAsyncResult { get; init; }
    public int GetSharedCollectionsAsyncInvokeCount { get; private set; }
    public IUserIdItrEntity GetSharedCollectionsAsyncLastArgs { get; private set; }

    public IOperationResponse<IEnumerable<ICollectionOufEntity>> GetAccessibleCollectionsAsyncResult { get; init; }
    public int GetAccessibleCollectionsAsyncInvokeCount { get; private set; }
    public IUserIdItrEntity GetAccessibleCollectionsAsyncLastArgs { get; private set; }

    public IOperationResponse<ICollectionOufEntity> RenameCollectionAsyncResult { get; init; }
    public int RenameCollectionAsyncInvokeCount { get; private set; }
    public IRenameCollectionItrEntity RenameCollectionAsyncLastEntity { get; private set; }

    public IOperationResponse<ICollectionOufEntity> UpdateCollectionVisibilityAsyncResult { get; init; }
    public int UpdateCollectionVisibilityAsyncInvokeCount { get; private set; }
    public IUpdateCollectionVisibilityItrEntity UpdateCollectionVisibilityAsyncLastEntity { get; private set; }

    public IOperationResponse<ICollectionOufEntity> GrantCollectionAccessAsyncResult { get; init; }
    public int GrantCollectionAccessAsyncInvokeCount { get; private set; }
    public IGrantCollectionAccessItrEntity GrantCollectionAccessAsyncLastEntity { get; private set; }

    public IOperationResponse<ICollectionOufEntity> RevokeCollectionAccessAsyncResult { get; init; }
    public int RevokeCollectionAccessAsyncInvokeCount { get; private set; }
    public IRevokeCollectionAccessItrEntity RevokeCollectionAccessAsyncLastEntity { get; private set; }

    public IOperationResponse<ICollectionOufEntity> DeleteCollectionAsyncResult { get; init; }
    public int DeleteCollectionAsyncInvokeCount { get; private set; }
    public IDeleteCollectionItrEntity DeleteCollectionAsyncLastEntity { get; private set; }

    public IOperationResponse<ICollectionOufEntity> TransferCollectionOwnershipAsyncResult { get; init; }
    public int TransferCollectionOwnershipAsyncInvokeCount { get; private set; }
    public ITransferCollectionOwnershipItrEntity TransferCollectionOwnershipAsyncLastEntity { get; private set; }

    public Task<IOperationResponse<ICollectionOufEntity>> CreateCollectionAsync(ICollectionItrEntity entity)
    {
        CreateCollectionAsyncInvokeCount++;
        CreateCollectionAsyncLastEntity = entity;
        return Task.FromResult(CreateCollectionAsyncResult);
    }

    public Task<IOperationResponse<ICollectionOufEntity>> GetDefaultCollectionAsync(IOwnerIdItrEntity args)
    {
        GetDefaultCollectionAsyncInvokeCount++;
        GetDefaultCollectionAsyncLastArgs = args;
        return Task.FromResult(GetDefaultCollectionAsyncResult);
    }

    public Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetCollectionsByOwnerAsync(IOwnerIdItrEntity args)
    {
        GetCollectionsByOwnerAsyncInvokeCount++;
        GetCollectionsByOwnerAsyncLastArgs = args;
        return Task.FromResult(GetCollectionsByOwnerAsyncResult);
    }

    public Task<IOperationResponse<ICollectionOufEntity>> GetCollectionByIdAsync(ICollectionIdItrEntity args)
    {
        GetCollectionByIdAsyncInvokeCount++;
        GetCollectionByIdAsyncLastArgs = args;
        return Task.FromResult(GetCollectionByIdAsyncResult);
    }

    public Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetSharedCollectionsAsync(IUserIdItrEntity args)
    {
        GetSharedCollectionsAsyncInvokeCount++;
        GetSharedCollectionsAsyncLastArgs = args;
        return Task.FromResult(GetSharedCollectionsAsyncResult);
    }

    public Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetAccessibleCollectionsAsync(IUserIdItrEntity args)
    {
        GetAccessibleCollectionsAsyncInvokeCount++;
        GetAccessibleCollectionsAsyncLastArgs = args;
        return Task.FromResult(GetAccessibleCollectionsAsyncResult);
    }

    public Task<IOperationResponse<ICollectionOufEntity>> RenameCollectionAsync(IRenameCollectionItrEntity entity)
    {
        RenameCollectionAsyncInvokeCount++;
        RenameCollectionAsyncLastEntity = entity;
        return Task.FromResult(RenameCollectionAsyncResult);
    }

    public Task<IOperationResponse<ICollectionOufEntity>> UpdateCollectionVisibilityAsync(IUpdateCollectionVisibilityItrEntity entity)
    {
        UpdateCollectionVisibilityAsyncInvokeCount++;
        UpdateCollectionVisibilityAsyncLastEntity = entity;
        return Task.FromResult(UpdateCollectionVisibilityAsyncResult);
    }

    public Task<IOperationResponse<ICollectionOufEntity>> GrantCollectionAccessAsync(IGrantCollectionAccessItrEntity entity)
    {
        GrantCollectionAccessAsyncInvokeCount++;
        GrantCollectionAccessAsyncLastEntity = entity;
        return Task.FromResult(GrantCollectionAccessAsyncResult);
    }

    public Task<IOperationResponse<ICollectionOufEntity>> RevokeCollectionAccessAsync(IRevokeCollectionAccessItrEntity entity)
    {
        RevokeCollectionAccessAsyncInvokeCount++;
        RevokeCollectionAccessAsyncLastEntity = entity;
        return Task.FromResult(RevokeCollectionAccessAsyncResult);
    }

    public Task<IOperationResponse<ICollectionOufEntity>> DeleteCollectionAsync(IDeleteCollectionItrEntity entity)
    {
        DeleteCollectionAsyncInvokeCount++;
        DeleteCollectionAsyncLastEntity = entity;
        return Task.FromResult(DeleteCollectionAsyncResult);
    }

    public Task<IOperationResponse<ICollectionOufEntity>> TransferCollectionOwnershipAsync(ITransferCollectionOwnershipItrEntity entity)
    {
        TransferCollectionOwnershipAsyncInvokeCount++;
        TransferCollectionOwnershipAsyncLastEntity = entity;
        return Task.FromResult(TransferCollectionOwnershipAsyncResult);
    }
}
