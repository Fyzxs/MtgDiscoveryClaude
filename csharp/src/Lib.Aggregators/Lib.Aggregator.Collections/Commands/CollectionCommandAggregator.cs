using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Aggregator.Collections.Apis;
using Lib.Aggregator.Collections.Commands.Mappers;
using Lib.Aggregator.Collections.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.DataModels.Entities.Oufs.Collections;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Collections.Commands;

internal sealed class CollectionCommandAggregator : ICollectionCommandAggregatorService
{
    private readonly ICollectionsAdapterService _adapterService;
    private readonly ICollectionExtToOufMapper _extToOufMapper;
    private readonly ICollectionItrToXfrMapper _collectionItrToXfrMapper;
    private readonly IRenameCollectionItrToXfrMapper _renameCollectionItrToXfrMapper;
    private readonly IUpdateCollectionVisibilityItrToXfrMapper _updateVisibilityItrToXfrMapper;
    private readonly IGrantCollectionAccessItrToXfrMapper _grantAccessItrToXfrMapper;
    private readonly IRevokeCollectionAccessItrToXfrMapper _revokeAccessItrToXfrMapper;
    private readonly IDeleteCollectionItrToXfrMapper _deleteCollectionItrToXfrMapper;
    private readonly ITransferCollectionOwnershipItrToXfrMapper _transferOwnershipItrToXfrMapper;

    public CollectionCommandAggregator(ILogger logger) : this(
        new CollectionsAdapterService(logger),
        new CollectionExtToOufMapper(),
        new CollectionItrToXfrMapper(),
        new RenameCollectionItrToXfrMapper(),
        new UpdateCollectionVisibilityItrToXfrMapper(),
        new GrantCollectionAccessItrToXfrMapper(),
        new RevokeCollectionAccessItrToXfrMapper(),
        new DeleteCollectionItrToXfrMapper(),
        new TransferCollectionOwnershipItrToXfrMapper())
    { }

    private CollectionCommandAggregator(
        ICollectionsAdapterService adapterService,
        ICollectionExtToOufMapper extToOufMapper,
        ICollectionItrToXfrMapper collectionItrToXfrMapper,
        IRenameCollectionItrToXfrMapper renameCollectionItrToXfrMapper,
        IUpdateCollectionVisibilityItrToXfrMapper updateVisibilityItrToXfrMapper,
        IGrantCollectionAccessItrToXfrMapper grantAccessItrToXfrMapper,
        IRevokeCollectionAccessItrToXfrMapper revokeAccessItrToXfrMapper,
        IDeleteCollectionItrToXfrMapper deleteCollectionItrToXfrMapper,
        ITransferCollectionOwnershipItrToXfrMapper transferOwnershipItrToXfrMapper)
    {
        _adapterService = adapterService;
        _extToOufMapper = extToOufMapper;
        _collectionItrToXfrMapper = collectionItrToXfrMapper;
        _renameCollectionItrToXfrMapper = renameCollectionItrToXfrMapper;
        _updateVisibilityItrToXfrMapper = updateVisibilityItrToXfrMapper;
        _grantAccessItrToXfrMapper = grantAccessItrToXfrMapper;
        _revokeAccessItrToXfrMapper = revokeAccessItrToXfrMapper;
        _deleteCollectionItrToXfrMapper = deleteCollectionItrToXfrMapper;
        _transferOwnershipItrToXfrMapper = transferOwnershipItrToXfrMapper;
    }

    public async Task<IOperationResponse<ICollectionOufEntity>> CreateCollectionAsync(ICollectionItrEntity entity, CancellationToken cancellationToken)
    {
        ICollectionXfrEntity xfrEntity = await _collectionItrToXfrMapper.Map(entity).ConfigureAwait(false);

        IOperationResponse<CollectionExtEntity> response = await _adapterService
            .CreateCollectionAsync(xfrEntity, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(response.OuterException);
        }

        ICollectionOufEntity oufEntity = await _extToOufMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<ICollectionOufEntity>(oufEntity);
    }

    public async Task<IOperationResponse<ICollectionOufEntity>> RenameCollectionAsync(IRenameCollectionItrEntity entity, CancellationToken cancellationToken)
    {
        IRenameCollectionXfrEntity xfrEntity = await _renameCollectionItrToXfrMapper.Map(entity).ConfigureAwait(false);

        IOperationResponse<CollectionExtEntity> response = await _adapterService
            .RenameCollectionAsync(xfrEntity, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(response.OuterException);
        }

        ICollectionOufEntity oufEntity = await _extToOufMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<ICollectionOufEntity>(oufEntity);
    }

    public async Task<IOperationResponse<ICollectionOufEntity>> UpdateCollectionVisibilityAsync(IUpdateCollectionVisibilityItrEntity entity, CancellationToken cancellationToken)
    {
        IUpdateCollectionVisibilityXfrEntity xfrEntity = await _updateVisibilityItrToXfrMapper.Map(entity).ConfigureAwait(false);

        IOperationResponse<CollectionExtEntity> response = await _adapterService
            .UpdateCollectionVisibilityAsync(xfrEntity, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(response.OuterException);
        }

        ICollectionOufEntity oufEntity = await _extToOufMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<ICollectionOufEntity>(oufEntity);
    }

    public async Task<IOperationResponse<ICollectionOufEntity>> GrantCollectionAccessAsync(IGrantCollectionAccessItrEntity entity, CancellationToken cancellationToken)
    {
        IGrantCollectionAccessXfrEntity xfrEntity = await _grantAccessItrToXfrMapper.Map(entity).ConfigureAwait(false);

        IOperationResponse<CollectionExtEntity> response = await _adapterService
            .GrantCollectionAccessAsync(xfrEntity, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(response.OuterException);
        }

        ICollectionOufEntity oufEntity = await _extToOufMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<ICollectionOufEntity>(oufEntity);
    }

    public async Task<IOperationResponse<ICollectionOufEntity>> RevokeCollectionAccessAsync(IRevokeCollectionAccessItrEntity entity, CancellationToken cancellationToken)
    {
        IRevokeCollectionAccessXfrEntity xfrEntity = await _revokeAccessItrToXfrMapper.Map(entity).ConfigureAwait(false);

        IOperationResponse<CollectionExtEntity> response = await _adapterService
            .RevokeCollectionAccessAsync(xfrEntity, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(response.OuterException);
        }

        ICollectionOufEntity oufEntity = await _extToOufMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<ICollectionOufEntity>(oufEntity);
    }

    public async Task<IOperationResponse<ICollectionOufEntity>> DeleteCollectionAsync(IDeleteCollectionItrEntity entity, CancellationToken cancellationToken)
    {
        IDeleteCollectionXfrEntity xfrEntity = await _deleteCollectionItrToXfrMapper.Map(entity).ConfigureAwait(false);

        IOperationResponse<CollectionExtEntity> response = await _adapterService
            .DeleteCollectionAsync(xfrEntity, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(response.OuterException);
        }

        ICollectionOufEntity oufEntity = await _extToOufMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<ICollectionOufEntity>(oufEntity);
    }

    public async Task<IOperationResponse<ICollectionOufEntity>> TransferCollectionOwnershipAsync(ITransferCollectionOwnershipItrEntity entity, CancellationToken cancellationToken)
    {
        ITransferCollectionOwnershipXfrEntity xfrEntity = await _transferOwnershipItrToXfrMapper.Map(entity).ConfigureAwait(false);

        IOperationResponse<CollectionExtEntity> response = await _adapterService
            .TransferCollectionOwnershipAsync(xfrEntity, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(response.OuterException);
        }

        ICollectionOufEntity oufEntity = await _extToOufMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<ICollectionOufEntity>(oufEntity);
    }
}
