using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Enrichments;

internal interface IUserCardEnrichment
{
    Task Enrich(List<CardItemOutEntity> outEntities, IUserIdArgEntity args);
    Task EnrichBySet(List<CardItemOutEntity> outEntities, IUserCardsSetItrEntity context);
    Task EnrichByArtist(List<CardItemOutEntity> outEntities, IUserCardsArtistItrEntity context);
    Task EnrichByName(List<CardItemOutEntity> outEntities, IUserCardsNameItrEntity context);
}
