using System.Collections.Generic;
using System.Threading;
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

    public async Task<IOperationResponse<ICollectionOufEntity>> CreateCollectionAsync(ICollectionItrEntity entity, CancellationToken cancellationToken) => await _commandService.CreateCollectionAsync(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> RenameCollectionAsync(IRenameCollectionItrEntity entity, CancellationToken cancellationToken) => await _commandService.RenameCollectionAsync(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> UpdateCollectionVisibilityAsync(IUpdateCollectionVisibilityItrEntity entity, CancellationToken cancellationToken) => await _commandService.UpdateCollectionVisibilityAsync(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> GrantCollectionAccessAsync(IGrantCollectionAccessItrEntity entity, CancellationToken cancellationToken) => await _commandService.GrantCollectionAccessAsync(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> RevokeCollectionAccessAsync(IRevokeCollectionAccessItrEntity entity, CancellationToken cancellationToken) => await _commandService.RevokeCollectionAccessAsync(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> DeleteCollectionAsync(IDeleteCollectionItrEntity entity, CancellationToken cancellationToken) => await _commandService.DeleteCollectionAsync(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> TransferCollectionOwnershipAsync(ITransferCollectionOwnershipItrEntity entity, CancellationToken cancellationToken) => await _commandService.TransferCollectionOwnershipAsync(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> GetDefaultCollectionAsync(IOwnerIdItrEntity args, CancellationToken cancellationToken) => await _queryService.GetDefaultCollectionAsync(args, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetCollectionsByOwnerAsync(IOwnerIdItrEntity args, CancellationToken cancellationToken) => await _queryService.GetCollectionsByOwnerAsync(args, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> GetCollectionByIdAsync(ICollectionIdItrEntity args, CancellationToken cancellationToken) => await _queryService.GetCollectionByIdAsync(args, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetSharedCollectionsAsync(IUserIdItrEntity args, CancellationToken cancellationToken) => await _queryService.GetSharedCollectionsAsync(args, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetAccessibleCollectionsAsync(IUserIdItrEntity args, CancellationToken cancellationToken) => await _queryService.GetAccessibleCollectionsAsync(args, cancellationToken).ConfigureAwait(false);
}
