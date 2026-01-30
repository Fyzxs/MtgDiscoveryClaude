using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Aggregator.Collections.Commands;
using Lib.Aggregator.Collections.Queries;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.DataModels.Entities.Oufs.Collections;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Collections.Apis;

public sealed class CollectionsAggregatorService : ICollectionsAggregatorService
{
    private readonly ICollectionCommandAggregatorService _commandAggregator;
    private readonly ICollectionQueryAggregatorService _queryAggregator;

    public CollectionsAggregatorService(ILogger logger) : this(
        new CollectionCommandAggregator(logger),
        new CollectionQueryAggregator(logger))
    { }

    private CollectionsAggregatorService(
        ICollectionCommandAggregatorService commandAggregator,
        ICollectionQueryAggregatorService queryAggregator)
    {
        _commandAggregator = commandAggregator;
        _queryAggregator = queryAggregator;
    }

    public Task<IOperationResponse<ICollectionOufEntity>> CreateCollectionAsync(ICollectionItrEntity entity) =>
        _commandAggregator.CreateCollectionAsync(entity);

    public Task<IOperationResponse<ICollectionOufEntity>> RenameCollectionAsync(IRenameCollectionItrEntity entity) =>
        _commandAggregator.RenameCollectionAsync(entity);

    public Task<IOperationResponse<ICollectionOufEntity>> UpdateCollectionVisibilityAsync(IUpdateCollectionVisibilityItrEntity entity) =>
        _commandAggregator.UpdateCollectionVisibilityAsync(entity);

    public Task<IOperationResponse<ICollectionOufEntity>> GrantCollectionAccessAsync(IGrantCollectionAccessItrEntity entity) =>
        _commandAggregator.GrantCollectionAccessAsync(entity);

    public Task<IOperationResponse<ICollectionOufEntity>> RevokeCollectionAccessAsync(IRevokeCollectionAccessItrEntity entity) =>
        _commandAggregator.RevokeCollectionAccessAsync(entity);

    public Task<IOperationResponse<ICollectionOufEntity>> DeleteCollectionAsync(IDeleteCollectionItrEntity entity) =>
        _commandAggregator.DeleteCollectionAsync(entity);

    public Task<IOperationResponse<ICollectionOufEntity>> TransferCollectionOwnershipAsync(ITransferCollectionOwnershipItrEntity entity) =>
        _commandAggregator.TransferCollectionOwnershipAsync(entity);

    public Task<IOperationResponse<ICollectionOufEntity>> GetDefaultCollectionAsync(string ownerId) =>
        _queryAggregator.GetDefaultCollectionAsync(ownerId);

    public Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetCollectionsByOwnerAsync(string ownerId) =>
        _queryAggregator.GetCollectionsByOwnerAsync(ownerId);

    public Task<IOperationResponse<ICollectionOufEntity>> GetCollectionByIdAsync(string collectionId, string ownerId) =>
        _queryAggregator.GetCollectionByIdAsync(collectionId, ownerId);

    public Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetSharedCollectionsAsync(string userId) =>
        _queryAggregator.GetSharedCollectionsAsync(userId);

    public Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetAccessibleCollectionsAsync(string userId) =>
        _queryAggregator.GetAccessibleCollectionsAsync(userId);
}
