using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.Sets;
using Lib.Shared.DataModels.Entities.Oufs.Sets;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.Sets.Queries;

internal interface IAllSetsAggregatorService
{
    Task<IOperationResponse<ISetItemCollectionOufEntity>> Execute(
        IAllSetsItrEntity input,
        CancellationToken cancellationToken);
}
