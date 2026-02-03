using System.Collections.Generic;
using System.Threading;
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

    public async Task<IOperationResponse<ICollectionOufEntity>> GetDefaultCollectionAsync(IOwnerIdItrEntity args, CancellationToken cancellationToken)
    {
        IOperationResponse<ICollectionOufEntity> response = await _adapterService
            .GetDefaultCollectionAsync(args, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(response.OuterException);
        }

        return new SuccessOperationResponse<ICollectionOufEntity>(response.ResponseData);
    }

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetCollectionsByOwnerAsync(IOwnerIdItrEntity args, CancellationToken cancellationToken)
    {
        IOperationResponse<IEnumerable<ICollectionOufEntity>> response = await _adapterService
            .GetCollectionsByOwnerAsync(args, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IEnumerable<ICollectionOufEntity>>(response.OuterException);
        }

        return new SuccessOperationResponse<IEnumerable<ICollectionOufEntity>>(response.ResponseData);
    }

    public async Task<IOperationResponse<ICollectionOufEntity>> GetCollectionByIdAsync(ICollectionIdItrEntity args, CancellationToken cancellationToken)
    {
        IOperationResponse<ICollectionOufEntity> response = await _adapterService
            .GetCollectionByIdAsync(args, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(response.OuterException);
        }

        return new SuccessOperationResponse<ICollectionOufEntity>(response.ResponseData);
    }

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetSharedCollectionsAsync(IUserIdItrEntity args, CancellationToken cancellationToken)
    {
        IOperationResponse<IEnumerable<ICollectionOufEntity>> response = await _adapterService
            .GetSharedCollectionsAsync(args, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IEnumerable<ICollectionOufEntity>>(response.OuterException);
        }

        return new SuccessOperationResponse<IEnumerable<ICollectionOufEntity>>(response.ResponseData);
    }

    public async Task<IOperationResponse<IEnumerable<ICollectionOufEntity>>> GetAccessibleCollectionsAsync(IUserIdItrEntity args, CancellationToken cancellationToken)
    {
        IOperationResponse<IEnumerable<ICollectionOufEntity>> response = await _adapterService
            .GetAccessibleCollectionsAsync(args, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IEnumerable<ICollectionOufEntity>>(response.OuterException);
        }

        return new SuccessOperationResponse<IEnumerable<ICollectionOufEntity>>(response.ResponseData);
    }
}
