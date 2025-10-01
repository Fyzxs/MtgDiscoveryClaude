using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.DataModels.Entities.Outs.Sets;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Apis;

public interface ISetEntryService
{
    Task<IOperationResponse<List<ScryfallSetOutEntity>>> SetsByIdsAsync(ISetIdsArgEntity setIds);
    Task<IOperationResponse<List<ScryfallSetOutEntity>>> SetsByCodeAsync(ISetCodesArgEntity setCodes);
    Task<IOperationResponse<List<ScryfallSetOutEntity>>> AllSetsAsync();
}
