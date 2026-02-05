using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis;
using Lib.Aggregator.Collections.Apis;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.DataModels.Entities.Oufs.Collections;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Collections.Commands;

internal sealed class CollectionCommandAggregator : ICollectionCommandAggregatorService
{
    private readonly ICollectionsAdapterService _adapterService;

    public CollectionCommandAggregator(ILogger logger) : this(new CollectionsAdapterService(logger)) { }

    private CollectionCommandAggregator(ICollectionsAdapterService adapterService) => _adapterService = adapterService;

    public async Task<IOperationResponse<ICollectionOufEntity>> CreateCollectionAsync(ICollectionItrEntity entity, CancellationToken cancellationToken)
    {
        IOperationResponse<ICollectionOufEntity> response = await _adapterService
            .CreateCollectionAsync(entity, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(response.OuterException);
        }

        return new SuccessOperationResponse<ICollectionOufEntity>(response.ResponseData);
    }

    public async Task<IOperationResponse<ICollectionOufEntity>> RenameCollectionAsync(IRenameCollectionItrEntity entity, CancellationToken cancellationToken)
    {
        IOperationResponse<ICollectionOufEntity> response = await _adapterService
            .RenameCollectionAsync(entity, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(response.OuterException);
        }

        return new SuccessOperationResponse<ICollectionOufEntity>(response.ResponseData);
    }

    public async Task<IOperationResponse<ICollectionOufEntity>> UpdateCollectionVisibilityAsync(IUpdateCollectionVisibilityItrEntity entity, CancellationToken cancellationToken)
    {
        IOperationResponse<ICollectionOufEntity> response = await _adapterService
            .UpdateCollectionVisibilityAsync(entity, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(response.OuterException);
        }

        return new SuccessOperationResponse<ICollectionOufEntity>(response.ResponseData);
    }

    public async Task<IOperationResponse<ICollectionOufEntity>> GrantCollectionAccessAsync(IGrantCollectionAccessItrEntity entity, CancellationToken cancellationToken)
    {
        IOperationResponse<ICollectionOufEntity> response = await _adapterService
            .GrantCollectionAccessAsync(entity, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(response.OuterException);
        }

        return new SuccessOperationResponse<ICollectionOufEntity>(response.ResponseData);
    }

    public async Task<IOperationResponse<ICollectionOufEntity>> RevokeCollectionAccessAsync(IRevokeCollectionAccessItrEntity entity, CancellationToken cancellationToken)
    {
        IOperationResponse<ICollectionOufEntity> response = await _adapterService
            .RevokeCollectionAccessAsync(entity, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(response.OuterException);
        }

        return new SuccessOperationResponse<ICollectionOufEntity>(response.ResponseData);
    }

    public async Task<IOperationResponse<ICollectionOufEntity>> DeleteCollectionAsync(IDeleteCollectionItrEntity entity, CancellationToken cancellationToken)
    {
        IOperationResponse<ICollectionOufEntity> response = await _adapterService
            .DeleteCollectionAsync(entity, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(response.OuterException);
        }

        return new SuccessOperationResponse<ICollectionOufEntity>(response.ResponseData);
    }

    public async Task<IOperationResponse<ICollectionOufEntity>> TransferCollectionOwnershipAsync(ITransferCollectionOwnershipItrEntity entity, CancellationToken cancellationToken)
    {
        IOperationResponse<ICollectionOufEntity> response = await _adapterService
            .TransferCollectionOwnershipAsync(entity, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(response.OuterException);
        }

        return new SuccessOperationResponse<ICollectionOufEntity>(response.ResponseData);
    }
}
