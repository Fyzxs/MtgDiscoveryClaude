using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.Sets;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Sets;

internal interface ISetsByCodeEntryService
{
    Task<IOperationResponse<List<SetItemOutEntity>>> Execute(
        ISetCodesArgEntity input,
        CancellationToken cancellationToken);
}
