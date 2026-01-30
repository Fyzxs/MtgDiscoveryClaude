using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Domain.Collections.Commands;
using Lib.Domain.Collections.Queries;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.DataModels.Entities.Itrs.User;
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

    public Task<IOperationResponse<ICollectionOufEntity>> GetDefaultCollectionAsync(IOwnerIdItrEntity args) =>
        _queryService.GetDefaultCollectionAsync(args);

    public Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetCollectionsByOwnerAsync(IOwnerIdItrEntity args) =>
        _queryService.GetCollectionsByOwnerAsync(args);

    public Task<IOperationResponse<ICollectionOufEntity>> GetCollectionByIdAsync(ICollectionIdItrEntity args) =>
        _queryService.GetCollectionByIdAsync(args);

    public Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetSharedCollectionsAsync(IUserIdItrEntity args) =>
        _queryService.GetSharedCollectionsAsync(args);

    public Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetAccessibleCollectionsAsync(IUserIdItrEntity args) =>
        _queryService.GetAccessibleCollectionsAsync(args);
}
