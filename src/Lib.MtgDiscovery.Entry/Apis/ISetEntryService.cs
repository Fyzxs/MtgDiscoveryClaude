using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.Sets;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Apis;

public interface ISetEntryService
{
    Task<IOperationResponse<List<SetItemOutEntity>>> SetsByIdsAsync(ISetIdsArgEntity setIds);
    Task<IOperationResponse<List<SetItemOutEntity>>> SetsByCodeAsync(ISetCodesArgEntity setCodes);
    Task<IOperationResponse<List<SetItemOutEntity>>> AllSetsAsync(IAllSetsArgEntity args);
}
