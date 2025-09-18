using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Apis;

public interface ISetEntryService
{
    Task<IOperationResponse<ISetItemCollectionOufEntity>> SetsByIdsAsync(ISetIdsArgEntity setIds);
    Task<IOperationResponse<ISetItemCollectionOufEntity>> SetsByCodeAsync(ISetCodesArgEntity setCodes);
    Task<IOperationResponse<ISetItemCollectionOufEntity>> AllSetsAsync();
}
