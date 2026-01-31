using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis;
using Lib.Aggregator.Collections.Apis;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.DataModels.Entities.Itrs.User;
using Lib.Shared.DataModels.Entities.Oufs.Collections;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Collections.Queries;

internal sealed class CollectionQueryAggregator : ICollectionQueryAggregatorService
{
    private readonly ICollectionsAdapterService _adapterService;

    public CollectionQueryAggregator(ILogger logger) : this(new CollectionsAdapterService(logger)) { }

    private CollectionQueryAggregator(ICollectionsAdapterService adapterService) => _adapterService = adapterService;

    public async Task<IOperationResponse<ICollectionOufEntity>> GetDefaultCollectionAsync(IOwnerIdItrEntity args)
    {
        IOperationResponse<ICollectionOufEntity> response = await _adapterService
            .GetDefaultCollectionAsync(args)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(response.OuterException);
        }

        return new SuccessOperationResponse<ICollectionOufEntity>(response.ResponseData);
    }

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetCollectionsByOwnerAsync(IOwnerIdItrEntity args)
    {
        IOperationResponse<IEnumerable<ICollectionOufEntity>> response = await _adapterService
            .GetCollectionsByOwnerAsync(args)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IEnumerable<ICollectionOufEntity>>(response.OuterException);
        }

        return new SuccessOperationResponse<IEnumerable<ICollectionOufEntity>>(response.ResponseData);
    }

    public async Task<IOperationResponse<ICollectionOufEntity>> GetCollectionByIdAsync(ICollectionIdItrEntity args)
    {
        IOperationResponse<ICollectionOufEntity> response = await _adapterService
            .GetCollectionByIdAsync(args)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(response.OuterException);
        }

        return new SuccessOperationResponse<ICollectionOufEntity>(response.ResponseData);
    }

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetSharedCollectionsAsync(IUserIdItrEntity args)
    {
        IOperationResponse<IEnumerable<ICollectionOufEntity>> response = await _adapterService
            .GetSharedCollectionsAsync(args)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IEnumerable<ICollectionOufEntity>>(response.OuterException);
        }

        return new SuccessOperationResponse<IEnumerable<ICollectionOufEntity>>(response.ResponseData);
    }

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetAccessibleCollectionsAsync(IUserIdItrEntity args)
    {
        IOperationResponse<IEnumerable<ICollectionOufEntity>> response = await _adapterService
            .GetAccessibleCollectionsAsync(args)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IEnumerable<ICollectionOufEntity>>(response.OuterException);
        }

        return new SuccessOperationResponse<IEnumerable<ICollectionOufEntity>>(response.ResponseData);
    }
}
