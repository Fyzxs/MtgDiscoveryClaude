using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Domain.Collections.Commands;
using Lib.Domain.Collections.Queries;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.DataModels.Entities.Oufs.Collections;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.Collections.Apis;

public sealed class CollectionsDomainService : ICollectionsDomainService
{
    private readonly ICollectionCommandDomainService _commandService;
    private readonly ICollectionQueryDomainService _queryService;

    public CollectionsDomainService(ILogger logger) : this(
        new CollectionCommandDomainService(logger),
        new CollectionQueryDomainService(logger))
    { }

    private CollectionsDomainService(
        ICollectionCommandDomainService commandService,
        ICollectionQueryDomainService queryService)
    {
        _commandService = commandService;
        _queryService = queryService;
    }

    public Task<IOperationResponse<ICollectionOufEntity>> CreateCollectionAsync(ICollectionItrEntity entity) =>
        _commandService.CreateCollectionAsync(entity);

    public Task<IOperationResponse<ICollectionOufEntity>> RenameCollectionAsync(IRenameCollectionItrEntity entity) =>
        _commandService.RenameCollectionAsync(entity);

    public Task<IOperationResponse<ICollectionOufEntity>> UpdateCollectionVisibilityAsync(IUpdateCollectionVisibilityItrEntity entity) =>
        _commandService.UpdateCollectionVisibilityAsync(entity);

    public Task<IOperationResponse<ICollectionOufEntity>> GrantCollectionAccessAsync(IGrantCollectionAccessItrEntity entity) =>
        _commandService.GrantCollectionAccessAsync(entity);

    public Task<IOperationResponse<ICollectionOufEntity>> RevokeCollectionAccessAsync(IRevokeCollectionAccessItrEntity entity) =>
        _commandService.RevokeCollectionAccessAsync(entity);

    public Task<IOperationResponse<ICollectionOufEntity>> DeleteCollectionAsync(IDeleteCollectionItrEntity entity) =>
        _commandService.DeleteCollectionAsync(entity);

    public Task<IOperationResponse<ICollectionOufEntity>> TransferCollectionOwnershipAsync(ITransferCollectionOwnershipItrEntity entity) =>
        _commandService.TransferCollectionOwnershipAsync(entity);

    public Task<IOperationResponse<ICollectionOufEntity>> GetDefaultCollectionAsync(string ownerId) =>
        _queryService.GetDefaultCollectionAsync(ownerId);

    public Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetCollectionsByOwnerAsync(string ownerId) =>
        _queryService.GetCollectionsByOwnerAsync(ownerId);

    public Task<IOperationResponse<ICollectionOufEntity>> GetCollectionByIdAsync(string collectionId, string ownerId) =>
        _queryService.GetCollectionByIdAsync(collectionId, ownerId);

    public Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetSharedCollectionsAsync(string userId) =>
        _queryService.GetSharedCollectionsAsync(userId);

    public Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetAccessibleCollectionsAsync(string userId) =>
        _queryService.GetAccessibleCollectionsAsync(userId);
}
