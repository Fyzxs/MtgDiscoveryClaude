using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.Sets;
using Lib.Shared.DataModels.Entities.Oufs.Sets;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.Sets.Apis;

public interface ISetsQueryAggregatorService
{
    Task<IOperationResponse<ISetItemCollectionOufEntity>> SetsAsync(
        ISetIdsItrEntity setIds,
        CancellationToken cancellationToken);

    Task<IOperationResponse<ISetItemCollectionOufEntity>> SetsByCodeAsync(
        ISetCodesItrEntity setCodes,
        CancellationToken cancellationToken);

    Task<IOperationResponse<ISetItemCollectionOufEntity>> AllSetsAsync(
        IAllSetsItrEntity allSets,
        CancellationToken cancellationToken);
}
