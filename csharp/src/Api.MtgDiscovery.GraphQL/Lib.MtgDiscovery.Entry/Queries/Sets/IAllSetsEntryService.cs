using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.Sets;
using Lib.Shared.DataModels.Entities.Args.Sets;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Sets;

internal interface IAllSetsEntryService
{
    Task<IOperationResponse<List<SetItemOutEntity>>> Execute(
        IAllSetsArgEntity input,
        CancellationToken cancellationToken);
}
