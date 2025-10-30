using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Entities.Outs.Sets;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.Invocation.Services;

namespace Lib.MtgDiscovery.Entry.Queries.Sets;

internal interface IAllSetsEntryService : IOperationResponseService<IAllSetsArgEntity, List<SetItemOutEntity>>
{
}
