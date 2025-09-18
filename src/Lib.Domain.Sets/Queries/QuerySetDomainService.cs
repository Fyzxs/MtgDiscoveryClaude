using System.Threading.Tasks;
using Lib.Aggregator.Sets.Apis;
using Lib.Domain.Sets.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.Sets.Queries;

internal sealed class QuerySetDomainService : ISetDomainService
{
    private readonly ISetAggregatorService _setAggregatorService;

    public QuerySetDomainService(ILogger logger) : this(new SetAggregatorService(logger))
    { }

    private QuerySetDomainService(ISetAggregatorService setAggregatorService) => _setAggregatorService = setAggregatorService;

    public async Task<IOperationResponse<ISetItemCollectionItrEntity>> SetsAsync(ISetIdsItrEntity setIds) => await _setAggregatorService.SetsAsync(setIds).ConfigureAwait(false);

    public async Task<IOperationResponse<ISetItemCollectionItrEntity>> SetsByCodeAsync(ISetCodesItrEntity setCodes) => await _setAggregatorService.SetsByCodeAsync(setCodes).ConfigureAwait(false);

    public async Task<IOperationResponse<ISetItemCollectionItrEntity>> AllSetsAsync() => await _setAggregatorService.AllSetsAsync().ConfigureAwait(false);
}
