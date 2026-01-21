using System.Threading.Tasks;
using Lib.Aggregator.Sets.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Sets.Queries;

internal sealed class SetsQueryAggregator : ISetAggregatorService
{
    private readonly ISetsByIdAggregatorService _setsByIdOperations;
    private readonly ISetsByCodeAggregatorService _setsByCodeOperations;
    private readonly IAllSetsAggregatorService _allSetsOperations;

    public SetsQueryAggregator(ILogger logger) : this(
        new SetsByIdAggregatorService(logger),
        new SetsByCodeAggregatorService(logger),
        new AllSetsAggregatorService(logger))
    { }

    private SetsQueryAggregator(
        ISetsByIdAggregatorService setsByIdOperations,
        ISetsByCodeAggregatorService setsByCodeOperations,
        IAllSetsAggregatorService allSetsOperations)
    {
        _setsByIdOperations = setsByIdOperations;
        _setsByCodeOperations = setsByCodeOperations;
        _allSetsOperations = allSetsOperations;
    }

    public Task<IOperationResponse<ISetItemCollectionOufEntity>> SetsAsync(ISetIdsItrEntity args) => _setsByIdOperations.Execute(args);

    public Task<IOperationResponse<ISetItemCollectionOufEntity>> SetsByCodeAsync(ISetCodesItrEntity args) => _setsByCodeOperations.Execute(args);

    public Task<IOperationResponse<ISetItemCollectionOufEntity>> AllSetsAsync(IAllSetsItrEntity allSets) => _allSetsOperations.Execute(allSets);
}
