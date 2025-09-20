using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.DataModels.Entities.Outs.Sets;

namespace Lib.MtgDiscovery.Entry.Queries.Sets;

internal interface IAllSetsEntryService : IOperationResponseService<NoArgsEntity, List<ScryfallSetOutEntity>>
{
}