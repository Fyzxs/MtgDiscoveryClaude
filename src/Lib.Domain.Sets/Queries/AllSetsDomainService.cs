using System.Threading.Tasks;
using Lib.Aggregator.Sets.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.Sets.Queries;

/// <summary>
/// Single-method service for retrieving all sets.
/// Delegates to aggregator layer for data retrieval.
/// </summary>
internal sealed class AllSetsDomainService : IAllSetsDomainService
{
    private readonly ISetAggregatorService _setAggregatorService;

    public AllSetsDomainService(ISetAggregatorService setAggregatorService) => _setAggregatorService = setAggregatorService;

    public async Task<IOperationResponse<ISetItemCollectionOufEntity>> Execute(INoArgsItrEntity input) => await _setAggregatorService.AllSetsAsync().ConfigureAwait(false);
}
