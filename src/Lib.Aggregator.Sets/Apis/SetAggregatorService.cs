using System.Threading.Tasks;
using Lib.Aggregator.Sets.Queries;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Sets.Apis;

public sealed class SetAggregatorService : ISetAggregatorService
{
    private readonly ISetAggregatorService _querySetAggregatorService;

    public SetAggregatorService(ILogger logger) : this(new QuerySetAggregatorService(logger))
    {
    }

    private SetAggregatorService(ISetAggregatorService querySetAggregatorService)
    {
        _querySetAggregatorService = querySetAggregatorService;
    }

    public async Task<IOperationResponse<ISetItemCollectionItrEntity>> SetsAsync(ISetIdsItrEntity setIds)
    {
        return await _querySetAggregatorService.SetsAsync(setIds).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<ISetItemCollectionItrEntity>> SetsByCodeAsync(ISetCodesItrEntity setCodes)
    {
        return await _querySetAggregatorService.SetsByCodeAsync(setCodes).ConfigureAwait(false);
    }
}