using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.Sets;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Queries.Enrichments;

internal interface IUserSetCollectionEnrichmentApplier
{
    Task Apply(List<SetItemOutEntity> outEntities, IEnumerable<IUserSetCardOufEntity> userSetCards);
}
