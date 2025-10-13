using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.Sets;

namespace Lib.MtgDiscovery.Entry.Queries.Sets;

internal interface ISetsByCodeEntryService : IOperationResponseService<ISetCodesArgEntity, List<ScryfallSetOutEntity>>
{
}
