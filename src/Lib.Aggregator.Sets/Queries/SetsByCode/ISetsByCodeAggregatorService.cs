using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.Sets.Queries.SetsByCode;

internal interface ISetsByCodeAggregatorService
{
    Task<IOperationResponse<ISetItemCollectionOufEntity>> SetsByCodeAsync(ISetCodesItrEntity args);
}
