using System.Threading;
using System.Threading.Tasks;
using Lib.Aggregator.Collections.Apis;
using Lib.Domain.Collections.Apis;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.DataModels.Entities.Oufs.Collections;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.Collections.Commands;

internal sealed class CollectionCommandDomainService : ICollectionCommandDomainService
{
    private readonly ICollectionsAggregatorService _aggregatorService;

    public CollectionCommandDomainService(ILogger logger) : this(new CollectionsAggregatorService(logger)) { }

    private CollectionCommandDomainService(ICollectionsAggregatorService aggregatorService) => _aggregatorService = aggregatorService;

    public async Task<IOperationResponse<ICollectionOufEntity>> CreateCollectionAsync(ICollectionItrEntity entity, CancellationToken cancellationToken) => await _aggregatorService.CreateCollectionAsync(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> RenameCollectionAsync(IRenameCollectionItrEntity entity, CancellationToken cancellationToken) => await _aggregatorService.RenameCollectionAsync(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> UpdateCollectionVisibilityAsync(IUpdateCollectionVisibilityItrEntity entity, CancellationToken cancellationToken) => await _aggregatorService.UpdateCollectionVisibilityAsync(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> GrantCollectionAccessAsync(IGrantCollectionAccessItrEntity entity, CancellationToken cancellationToken) => await _aggregatorService.GrantCollectionAccessAsync(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> RevokeCollectionAccessAsync(IRevokeCollectionAccessItrEntity entity, CancellationToken cancellationToken) => await _aggregatorService.RevokeCollectionAccessAsync(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> DeleteCollectionAsync(IDeleteCollectionItrEntity entity, CancellationToken cancellationToken) => await _aggregatorService.DeleteCollectionAsync(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> TransferCollectionOwnershipAsync(ITransferCollectionOwnershipItrEntity entity, CancellationToken cancellationToken) => await _aggregatorService.TransferCollectionOwnershipAsync(entity, cancellationToken).ConfigureAwait(false);
}
