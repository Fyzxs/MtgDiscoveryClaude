using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Aggregator.Collections.Apis;
using Lib.Domain.Collections.Apis;
using Lib.Shared.DataModels.Entities.Oufs.Collections;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.Collections.Queries;

internal sealed class CollectionQueryDomainService : ICollectionQueryDomainService
{
    private readonly ICollectionsAggregatorService _aggregatorService;

    public CollectionQueryDomainService(ILogger logger) : this(new CollectionsAggregatorService(logger)) { }

    private CollectionQueryDomainService(ICollectionsAggregatorService aggregatorService) =>
        _aggregatorService = aggregatorService;

    public async Task<IOperationResponse<ICollectionOufEntity>> GetDefaultCollectionAsync(string ownerId) =>
        await _aggregatorService.GetDefaultCollectionAsync(ownerId).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetCollectionsByOwnerAsync(string ownerId) =>
        await _aggregatorService.GetCollectionsByOwnerAsync(ownerId).ConfigureAwait(false);

    public async Task<IOperationResponse<ICollectionOufEntity>> GetCollectionByIdAsync(string collectionId, string ownerId) =>
        await _aggregatorService.GetCollectionByIdAsync(collectionId, ownerId).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetSharedCollectionsAsync(string userId) =>
        await _aggregatorService.GetSharedCollectionsAsync(userId).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetAccessibleCollectionsAsync(string userId) =>
        await _aggregatorService.GetAccessibleCollectionsAsync(userId).ConfigureAwait(false);
}
