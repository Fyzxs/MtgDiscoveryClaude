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

    public async Task<IOperationResponse<ICollectionOufEntity>> CreateCollectionAsync(ICollectionItrEntity entity) => await _commandService.CreateCollectionAsync(entity);

    public async Task<IOperationResponse<ICollectionOufEntity>> RenameCollectionAsync(IRenameCollectionItrEntity entity) => await _commandService.RenameCollectionAsync(entity);

    public async Task<IOperationResponse<ICollectionOufEntity>> UpdateCollectionVisibilityAsync(IUpdateCollectionVisibilityItrEntity entity) => await _commandService.UpdateCollectionVisibilityAsync(entity);

    public async Task<IOperationResponse<ICollectionOufEntity>> GrantCollectionAccessAsync(IGrantCollectionAccessItrEntity entity) => await _commandService.GrantCollectionAccessAsync(entity);

    public async Task<IOperationResponse<ICollectionOufEntity>> RevokeCollectionAccessAsync(IRevokeCollectionAccessItrEntity entity) => await _commandService.RevokeCollectionAccessAsync(entity);

    public async Task<IOperationResponse<ICollectionOufEntity>> DeleteCollectionAsync(IDeleteCollectionItrEntity entity) => await _commandService.DeleteCollectionAsync(entity);

    public async Task<IOperationResponse<ICollectionOufEntity>> TransferCollectionOwnershipAsync(ITransferCollectionOwnershipItrEntity entity) => await _commandService.TransferCollectionOwnershipAsync(entity);

    public async Task<IOperationResponse<ICollectionOufEntity>> GetDefaultCollectionAsync(IOwnerIdItrEntity args) => await _queryService.GetDefaultCollectionAsync(args);

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetCollectionsByOwnerAsync(IOwnerIdItrEntity args) => await _queryService.GetCollectionsByOwnerAsync(args);

    public async Task<IOperationResponse<ICollectionOufEntity>> GetCollectionByIdAsync(ICollectionIdItrEntity args) => await _queryService.GetCollectionByIdAsync(args);

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetSharedCollectionsAsync(IUserIdItrEntity args) => await _queryService.GetSharedCollectionsAsync(args);

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetAccessibleCollectionsAsync(IUserIdItrEntity args) => await _queryService.GetAccessibleCollectionsAsync(args);
}
