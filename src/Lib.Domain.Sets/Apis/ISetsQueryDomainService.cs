using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.Sets.Apis;

public interface ISetsQueryDomainService
{
    Task<IOperationResponse<ISetItemCollectionOufEntity>> SetsAsync(ISetIdsItrEntity setIds);
    Task<IOperationResponse<ISetItemCollectionOufEntity>> SetsByCodeAsync(ISetCodesItrEntity setCodes);
    Task<IOperationResponse<ISetItemCollectionOufEntity>> AllSetsAsync(IAllSetsItrEntity allSets);
}
