using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Apis;

public interface ISetEntryService
{
    Task<IOperationResponse<ISetItemCollectionItrEntity>> SetsByIdsAsync(ISetIdsArgEntity setIds);
    Task<IOperationResponse<ISetItemCollectionItrEntity>> SetsByCodeAsync(ISetCodesArgEntity setCodes);
    Task<IOperationResponse<ISetItemCollectionItrEntity>> AllSetsAsync();
}
