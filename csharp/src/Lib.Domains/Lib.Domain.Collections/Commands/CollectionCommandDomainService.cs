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

    public async Task<IOperationResponse<ICollectionOufEntity>> CreateCollectionAsync(ICollectionItrEntity entity) => await _aggregatorService.CreateCollectionAsync(entity).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> RenameCollectionAsync(IRenameCollectionItrEntity entity) => await _aggregatorService.RenameCollectionAsync(entity).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> UpdateCollectionVisibilityAsync(IUpdateCollectionVisibilityItrEntity entity) => await _aggregatorService.UpdateCollectionVisibilityAsync(entity).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> GrantCollectionAccessAsync(IGrantCollectionAccessItrEntity entity) => await _aggregatorService.GrantCollectionAccessAsync(entity).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> RevokeCollectionAccessAsync(IRevokeCollectionAccessItrEntity entity) => await _aggregatorService.RevokeCollectionAccessAsync(entity).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> DeleteCollectionAsync(IDeleteCollectionItrEntity entity) => await _aggregatorService.DeleteCollectionAsync(entity).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> TransferCollectionOwnershipAsync(ITransferCollectionOwnershipItrEntity entity) => await _aggregatorService.TransferCollectionOwnershipAsync(entity).ConfigureAwait(false);
}
