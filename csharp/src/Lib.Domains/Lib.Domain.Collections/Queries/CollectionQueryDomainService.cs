using System.Collections.Generic;
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

    public async Task<IOperationResponse<ICollectionOufEntity>> GetDefaultCollectionAsync(IOwnerIdItrEntity args) => await _aggregatorService.GetDefaultCollectionAsync(args).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetCollectionsByOwnerAsync(IOwnerIdItrEntity args) => await _aggregatorService.GetCollectionsByOwnerAsync(args).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> GetCollectionByIdAsync(ICollectionIdItrEntity args) => await _aggregatorService.GetCollectionByIdAsync(args).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetSharedCollectionsAsync(IUserIdItrEntity args) => await _aggregatorService.GetSharedCollectionsAsync(args).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetAccessibleCollectionsAsync(IUserIdItrEntity args) => await _aggregatorService.GetAccessibleCollectionsAsync(args).ConfigureAwait(false);
}
