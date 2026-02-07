using System.Threading;
using System.Threading.Tasks;
using Lib.Aggregator.Sets.Apis;
using Lib.Shared.DataModels.Entities.Itrs.Sets;
using Lib.Shared.DataModels.Entities.Oufs.Sets;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.Sets.Queries;

/// <summary>
/// Single-method service for retrieving all sets.
/// Delegates to aggregator layer for data retrieval.
/// </summary>
internal sealed class AllSetsDomainService : IAllSetsDomainService
{
    private readonly ISetAggregatorService _setAggregatorService;

    public AllSetsDomainService(ILogger logger) : this(new SetAggregatorService(logger))
    { }

    private AllSetsDomainService(ISetAggregatorService setAggregatorService) => _setAggregatorService = setAggregatorService;

    public async Task<IOperationResponse<ISetItemCollectionOufEntity>> Execute(
        IAllSetsItrEntity input,
        CancellationToken cancellationToken)
        => await _setAggregatorService.AllSetsAsync(input, cancellationToken).ConfigureAwait(false);
}
