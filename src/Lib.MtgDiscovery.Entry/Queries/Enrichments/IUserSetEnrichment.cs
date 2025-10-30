using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.Sets;
using Lib.Shared.DataModels.Entities.Args;

namespace Lib.MtgDiscovery.Entry.Queries.Enrichments;

internal interface IUserSetEnrichment
{
    Task Enrich(List<SetItemOutEntity> outEntities, IUserIdArgEntity args);
}
