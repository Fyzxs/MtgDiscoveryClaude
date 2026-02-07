using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Aggregator.Collections.Apis;
using Lib.Aggregator.Collections.Mappers;
using Lib.Aggregator.Collections.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.DataModels.Entities.Itrs.User;
using Lib.Shared.DataModels.Entities.Oufs.Collections;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Collections.Queries;

internal sealed class CollectionQueryAggregator : ICollectionQueryAggregatorService
{
    private readonly ICollectionsAdapterService _adapterService;
    private readonly ICollectionExtToOufMapper _extToOufMapper;
    private readonly IOwnerIdItrToXfrMapper _ownerIdItrToXfrMapper;
    private readonly ICollectionIdItrToXfrMapper _collectionIdItrToXfrMapper;
    private readonly IUserIdItrToXfrMapper _userIdItrToXfrMapper;

    public CollectionQueryAggregator(ILogger logger) : this(
        new CollectionsAdapterService(logger),
        new CollectionExtToOufMapper(),
        new OwnerIdItrToXfrMapper(),
        new CollectionIdItrToXfrMapper(),
        new UserIdItrToXfrMapper())
    { }

    private CollectionQueryAggregator(
        ICollectionsAdapterService adapterService,
        ICollectionExtToOufMapper extToOufMapper,
        IOwnerIdItrToXfrMapper ownerIdItrToXfrMapper,
        ICollectionIdItrToXfrMapper collectionIdItrToXfrMapper,
        IUserIdItrToXfrMapper userIdItrToXfrMapper)
    {
        _adapterService = adapterService;
        _extToOufMapper = extToOufMapper;
        _ownerIdItrToXfrMapper = ownerIdItrToXfrMapper;
        _collectionIdItrToXfrMapper = collectionIdItrToXfrMapper;
        _userIdItrToXfrMapper = userIdItrToXfrMapper;
    }

    public async Task<IOperationResponse<ICollectionOufEntity>> GetDefaultCollectionAsync(IOwnerIdItrEntity args, CancellationToken cancellationToken)
    {
        IOwnerIdXfrEntity xfrEntity = await _ownerIdItrToXfrMapper.Map(args).ConfigureAwait(false);

        IOperationResponse<CollectionExtEntity> response = await _adapterService
            .GetDefaultCollectionAsync(xfrEntity, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(response.OuterException);
        }

        ICollectionOufEntity oufEntity = await _extToOufMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<ICollectionOufEntity>(oufEntity);
    }

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetCollectionsByOwnerAsync(IOwnerIdItrEntity args, CancellationToken cancellationToken)
    {
        IOwnerIdXfrEntity xfrEntity = await _ownerIdItrToXfrMapper.Map(args).ConfigureAwait(false);

        IOperationResponse<IEnumerable<CollectionExtEntity>> response = await _adapterService
            .GetCollectionsByOwnerAsync(xfrEntity, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IEnumerable<ICollectionOufEntity>>(response.OuterException);
        }

        ICollectionOufEntity[] oufEntities = await Task.WhenAll(
            response.ResponseData.Select(ext => _extToOufMapper.Map(ext))).ConfigureAwait(false);

        return new SuccessOperationResponse<IEnumerable<ICollectionOufEntity>>(oufEntities);
    }

    public async Task<IOperationResponse<ICollectionOufEntity>> GetCollectionByIdAsync(ICollectionIdItrEntity args, CancellationToken cancellationToken)
    {
        ICollectionIdXfrEntity xfrEntity = await _collectionIdItrToXfrMapper.Map(args).ConfigureAwait(false);

        IOperationResponse<CollectionExtEntity> response = await _adapterService
            .GetCollectionByIdAsync(xfrEntity, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(response.OuterException);
        }

        ICollectionOufEntity oufEntity = await _extToOufMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<ICollectionOufEntity>(oufEntity);
    }

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetSharedCollectionsAsync(IUserIdItrEntity args, CancellationToken cancellationToken)
    {
        IUserIdXfrEntity xfrEntity = await _userIdItrToXfrMapper.Map(args).ConfigureAwait(false);

        IOperationResponse<IEnumerable<CollectionExtEntity>> response = await _adapterService
            .GetAccessibleCollectionsAsync(xfrEntity, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IEnumerable<ICollectionOufEntity>>(response.OuterException);
        }

        IEnumerable<CollectionExtEntity> sharedOnly = response.ResponseData
            .Where(c => c.OwnerId != args.UserId);

        ICollectionOufEntity[] oufEntities = await Task.WhenAll(
            sharedOnly.Select(ext => _extToOufMapper.Map(ext))).ConfigureAwait(false);

        return new SuccessOperationResponse<IEnumerable<ICollectionOufEntity>>(oufEntities);
    }

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetAccessibleCollectionsAsync(IUserIdItrEntity args, CancellationToken cancellationToken)
    {
        IUserIdXfrEntity xfrEntity = await _userIdItrToXfrMapper.Map(args).ConfigureAwait(false);

        IOperationResponse<IEnumerable<CollectionExtEntity>> response = await _adapterService
            .GetAccessibleCollectionsAsync(xfrEntity, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IEnumerable<ICollectionOufEntity>>(response.OuterException);
        }

        ICollectionOufEntity[] oufEntities = await Task.WhenAll(
            response.ResponseData.Select(ext => _extToOufMapper.Map(ext))).ConfigureAwait(false);

        return new SuccessOperationResponse<IEnumerable<ICollectionOufEntity>>(oufEntities);
    }
}
