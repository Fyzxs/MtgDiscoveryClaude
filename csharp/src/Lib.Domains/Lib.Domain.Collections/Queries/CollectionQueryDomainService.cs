using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Aggregator.Collections.Apis;
using Lib.Domain.Collections.Apis;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.DataModels.Entities.Itrs.User;
using Lib.Shared.DataModels.Entities.Oufs.Collections;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.Collections.Queries;

internal sealed class CollectionQueryDomainService : ICollectionQueryDomainService
{
    private readonly ICollectionsAggregatorService _aggregatorService;

    public CollectionQueryDomainService(ILogger logger) : this(new CollectionsAggregatorService(logger)) { }

    private CollectionQueryDomainService(ICollectionsAggregatorService aggregatorService) => _aggregatorService = aggregatorService;

    public async Task<IOperationResponse<ICollectionOufEntity>> GetDefaultCollectionAsync(IOwnerIdItrEntity args, CancellationToken cancellationToken) => await _aggregatorService.GetDefaultCollectionAsync(args, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetCollectionsByOwnerAsync(IOwnerIdItrEntity args, CancellationToken cancellationToken) => await _aggregatorService.GetCollectionsByOwnerAsync(args, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> GetCollectionByIdAsync(ICollectionIdItrEntity args, CancellationToken cancellationToken) => await _aggregatorService.GetCollectionByIdAsync(args, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetSharedCollectionsAsync(IUserIdItrEntity args, CancellationToken cancellationToken) => await _aggregatorService.GetSharedCollectionsAsync(args, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetAccessibleCollectionsAsync(IUserIdItrEntity args, CancellationToken cancellationToken) => await _aggregatorService.GetAccessibleCollectionsAsync(args, cancellationToken).ConfigureAwait(false);
}
