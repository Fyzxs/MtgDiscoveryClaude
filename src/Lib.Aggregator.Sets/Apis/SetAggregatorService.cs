using System.Threading.Tasks;
using Lib.Aggregator.Sets.Queries;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Sets.Apis;

public sealed class SetAggregatorService : ISetAggregatorService
{
    private readonly ISetsQueryAggregatorService _querySetAggregatorService;

    public SetAggregatorService(ILogger logger) : this(new SetsQueryAggregator(logger))
    { }

    private SetAggregatorService(ISetsQueryAggregatorService querySetAggregatorService) => _querySetAggregatorService = querySetAggregatorService;

    public async Task<IOperationResponse<ISetItemCollectionOufEntity>> SetsAsync(ISetIdsItrEntity setIds) => await _querySetAggregatorService.SetsAsync(setIds).ConfigureAwait(false);

    public async Task<IOperationResponse<ISetItemCollectionOufEntity>> SetsByCodeAsync(ISetCodesItrEntity setCodes) => await _querySetAggregatorService.SetsByCodeAsync(setCodes).ConfigureAwait(false);

    public async Task<IOperationResponse<ISetItemCollectionOufEntity>> AllSetsAsync(INoArgsItrEntity noArgs) => await _querySetAggregatorService.AllSetsAsync(noArgs).ConfigureAwait(false);
}
