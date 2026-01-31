using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Aggregator.Collections.Commands;
using Lib.Aggregator.Collections.Queries;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.DataModels.Entities.Itrs.User;
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

    public async Task<IOperationResponse<ICollectionOufEntity>> CreateCollectionAsync(ICollectionItrEntity entity) => await _commandAggregator.CreateCollectionAsync(entity);

    public async Task<IOperationResponse<ICollectionOufEntity>> RenameCollectionAsync(IRenameCollectionItrEntity entity) => await _commandAggregator.RenameCollectionAsync(entity);

    public async Task<IOperationResponse<ICollectionOufEntity>> UpdateCollectionVisibilityAsync(IUpdateCollectionVisibilityItrEntity entity) => await _commandAggregator.UpdateCollectionVisibilityAsync(entity);

    public async Task<IOperationResponse<ICollectionOufEntity>> GrantCollectionAccessAsync(IGrantCollectionAccessItrEntity entity) => await _commandAggregator.GrantCollectionAccessAsync(entity);

    public async Task<IOperationResponse<ICollectionOufEntity>> RevokeCollectionAccessAsync(IRevokeCollectionAccessItrEntity entity) => await _commandAggregator.RevokeCollectionAccessAsync(entity);

    public async Task<IOperationResponse<ICollectionOufEntity>> DeleteCollectionAsync(IDeleteCollectionItrEntity entity) => await _commandAggregator.DeleteCollectionAsync(entity);

    public async Task<IOperationResponse<ICollectionOufEntity>> TransferCollectionOwnershipAsync(ITransferCollectionOwnershipItrEntity entity) => await _commandAggregator.TransferCollectionOwnershipAsync(entity);

    public async Task<IOperationResponse<ICollectionOufEntity>> GetDefaultCollectionAsync(IOwnerIdItrEntity args) => await _queryAggregator.GetDefaultCollectionAsync(args);

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetCollectionsByOwnerAsync(IOwnerIdItrEntity args) => await _queryAggregator.GetCollectionsByOwnerAsync(args);

    public async Task<IOperationResponse<ICollectionOufEntity>> GetCollectionByIdAsync(ICollectionIdItrEntity args) => await _queryAggregator.GetCollectionByIdAsync(args);

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetSharedCollectionsAsync(IUserIdItrEntity args) => await _queryAggregator.GetSharedCollectionsAsync(args);

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetAccessibleCollectionsAsync(IUserIdItrEntity args) => await _queryAggregator.GetAccessibleCollectionsAsync(args);
}
