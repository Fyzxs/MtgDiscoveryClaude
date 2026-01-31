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

    public async Task<IOperationResponse<ICollectionOufEntity>> CreateCollectionAsync(ICollectionItrEntity entity)
    {
        IOperationResponse<ICollectionOufEntity> response = await _adapterService
            .CreateCollectionAsync(entity)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(response.OuterException);
        }

        return new SuccessOperationResponse<ICollectionOufEntity>(response.ResponseData);
    }

    public async Task<IOperationResponse<ICollectionOufEntity>> RenameCollectionAsync(IRenameCollectionItrEntity entity)
    {
        IOperationResponse<ICollectionOufEntity> response = await _adapterService
            .RenameCollectionAsync(entity)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(response.OuterException);
        }

        return new SuccessOperationResponse<ICollectionOufEntity>(response.ResponseData);
    }

    public async Task<IOperationResponse<ICollectionOufEntity>> UpdateCollectionVisibilityAsync(IUpdateCollectionVisibilityItrEntity entity)
    {
        IOperationResponse<ICollectionOufEntity> response = await _adapterService
            .UpdateCollectionVisibilityAsync(entity)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(response.OuterException);
        }

        return new SuccessOperationResponse<ICollectionOufEntity>(response.ResponseData);
    }

    public async Task<IOperationResponse<ICollectionOufEntity>> GrantCollectionAccessAsync(IGrantCollectionAccessItrEntity entity)
    {
        IOperationResponse<ICollectionOufEntity> response = await _adapterService
            .GrantCollectionAccessAsync(entity)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(response.OuterException);
        }

        return new SuccessOperationResponse<ICollectionOufEntity>(response.ResponseData);
    }

    public async Task<IOperationResponse<ICollectionOufEntity>> RevokeCollectionAccessAsync(IRevokeCollectionAccessItrEntity entity)
    {
        IOperationResponse<ICollectionOufEntity> response = await _adapterService
            .RevokeCollectionAccessAsync(entity)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(response.OuterException);
        }

        return new SuccessOperationResponse<ICollectionOufEntity>(response.ResponseData);
    }

    public async Task<IOperationResponse<ICollectionOufEntity>> DeleteCollectionAsync(IDeleteCollectionItrEntity entity)
    {
        IOperationResponse<ICollectionOufEntity> response = await _adapterService
            .DeleteCollectionAsync(entity)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(response.OuterException);
        }

        return new SuccessOperationResponse<ICollectionOufEntity>(response.ResponseData);
    }

    public async Task<IOperationResponse<ICollectionOufEntity>> TransferCollectionOwnershipAsync(ITransferCollectionOwnershipItrEntity entity)
    {
        IOperationResponse<ICollectionOufEntity> response = await _adapterService
            .TransferCollectionOwnershipAsync(entity)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICollectionOufEntity>(response.OuterException);
        }

        return new SuccessOperationResponse<ICollectionOufEntity>(response.ResponseData);
    }
}
