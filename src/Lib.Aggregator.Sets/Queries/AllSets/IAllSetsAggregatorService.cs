using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.Sets.Queries.AllSets;

internal interface IAllSetsAggregatorService
{
    Task<IOperationResponse<ISetItemCollectionOufEntity>> AllSetsAsync();
}
