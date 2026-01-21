using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Sets.Apis;
using Lib.Aggregator.Sets.Apis;
using Lib.Aggregator.Sets.Entities;
using Lib.Aggregator.Sets.Exceptions;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Sets.Queries;

internal sealed class QuerySetAggregatorService : ISetAggregatorService
{
    private readonly ISetAdapterService _setAdapterService;

    public QuerySetAggregatorService(ILogger logger) : this(new SetAdapterService(logger))
    { }

    private QuerySetAggregatorService(
        ISetAdapterService setAdapterService)
    {
        _setAdapterService = setAdapterService;
    }

    public async Task<IOperationResponse<ISetItemCollectionItrEntity>> SetsAsync(ISetIdsItrEntity args)
    {
        IOperationResponse<IEnumerable<ISetItemItrEntity>> response = await _setAdapterService.GetSetsByIdsAsync(args).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ISetItemCollectionItrEntity>(new AggregatorOperationException(System.Net.HttpStatusCode.InternalServerError, "Failed to retrieve sets by IDs", response.OuterException));
        }

        ICollection<ISetItemItrEntity> sets = [.. response.ResponseData];
        return new SuccessOperationResponse<ISetItemCollectionItrEntity>(new SetItemCollectionItrEntity { Data = sets });
    }

    public async Task<IOperationResponse<ISetItemCollectionItrEntity>> SetsByCodeAsync(ISetCodesItrEntity args)
    {
        IOperationResponse<IEnumerable<ISetItemItrEntity>> response = await _setAdapterService.GetSetsByCodesAsync(args).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ISetItemCollectionItrEntity>(new AggregatorOperationException(System.Net.HttpStatusCode.InternalServerError, "Failed to retrieve sets by codes", response.OuterException));
        }

        ICollection<ISetItemItrEntity> sets = [.. response.ResponseData];
        return new SuccessOperationResponse<ISetItemCollectionItrEntity>(new SetItemCollectionItrEntity { Data = sets });
    }

    public async Task<IOperationResponse<ISetItemCollectionItrEntity>> AllSetsAsync()
    {
        IOperationResponse<IEnumerable<ISetItemItrEntity>> response = await _setAdapterService.GetAllSetsAsync().ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ISetItemCollectionItrEntity>(new AggregatorOperationException(System.Net.HttpStatusCode.InternalServerError, "Failed to retrieve all sets", response.OuterException));
        }

        ICollection<ISetItemItrEntity> sets = [.. response.ResponseData];
        return new SuccessOperationResponse<ISetItemCollectionItrEntity>(new SetItemCollectionItrEntity { Data = sets });
    }
}
