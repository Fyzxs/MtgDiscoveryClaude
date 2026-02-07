using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.Sets;
using Lib.Shared.DataModels.Entities.Args.Sets;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Apis;

public interface ISetEntryService
{
    Task<IOperationResponse<List<SetItemOutEntity>>> SetsByIdsAsync(
        ISetIdsArgEntity setIds,
        CancellationToken cancellationToken);

    Task<IOperationResponse<List<SetItemOutEntity>>> SetsByCodeAsync(
        ISetCodesArgEntity setCodes,
        CancellationToken cancellationToken);

    Task<IOperationResponse<List<SetItemOutEntity>>> AllSetsAsync(
        IAllSetsArgEntity args,
        CancellationToken cancellationToken);
}
