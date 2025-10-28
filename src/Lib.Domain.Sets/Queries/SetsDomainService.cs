using System.Threading.Tasks;
using Lib.Aggregator.Sets.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.Sets.Queries;

/// <summary>
/// Single-method service for retrieving sets by ID collection.
/// Delegates to aggregator layer for data retrieval.
/// </summary>
internal sealed class SetsDomainService : ISetsDomainService
{
    private readonly ISetAggregatorService _setAggregatorService;

    public SetsDomainService(ISetAggregatorService setAggregatorService) => _setAggregatorService = setAggregatorService;

    public async Task<IOperationResponse<ISetItemCollectionOufEntity>> Execute(ISetIdsItrEntity input) => await _setAggregatorService.SetsAsync(input).ConfigureAwait(false);
}
