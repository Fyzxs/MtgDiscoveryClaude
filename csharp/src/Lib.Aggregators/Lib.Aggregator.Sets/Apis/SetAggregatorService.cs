using System.Threading;
using System.Threading.Tasks;
using Lib.Aggregator.Sets.Queries;
using Lib.Shared.DataModels.Entities.Itrs.Sets;
using Lib.Shared.DataModels.Entities.Oufs.Sets;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Sets.Apis;

public sealed class SetAggregatorService : ISetAggregatorService
{
    private readonly ISetsQueryAggregatorService _querySetAggregatorService;

    public SetAggregatorService(ILogger logger) : this(new SetsQueryAggregator(logger))
    { }

    private SetAggregatorService(ISetsQueryAggregatorService querySetAggregatorService) => _querySetAggregatorService = querySetAggregatorService;

    public async Task<IOperationResponse<ISetItemCollectionOufEntity>> SetsAsync(
        ISetIdsItrEntity setIds,
        CancellationToken cancellationToken)
        => await _querySetAggregatorService.SetsAsync(setIds, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ISetItemCollectionOufEntity>> SetsByCodeAsync(
        ISetCodesItrEntity setCodes,
        CancellationToken cancellationToken)
        => await _querySetAggregatorService.SetsByCodeAsync(setCodes, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ISetItemCollectionOufEntity>> AllSetsAsync(
        IAllSetsItrEntity allSets,
        CancellationToken cancellationToken)
        => await _querySetAggregatorService.AllSetsAsync(allSets, cancellationToken).ConfigureAwait(false);
}
