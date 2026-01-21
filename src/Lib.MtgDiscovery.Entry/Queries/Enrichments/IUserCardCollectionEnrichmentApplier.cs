using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;

namespace Lib.MtgDiscovery.Entry.Queries.Enrichments;

internal interface IUserCardCollectionEnrichmentApplier
{
    Task Apply(List<CardItemOutEntity> outEntities, IEnumerable<IUserCardOufEntity> userCards);
}
