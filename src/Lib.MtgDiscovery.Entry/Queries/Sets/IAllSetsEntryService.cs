using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.MtgDiscovery.Entry.Entities.Outs.Sets;
using Lib.Shared.Invocation.Services;

namespace Lib.MtgDiscovery.Entry.Queries.Sets;

internal interface IAllSetsEntryService : IOperationResponseService<NoArgsEntity, List<ScryfallSetOutEntity>>
{
}
