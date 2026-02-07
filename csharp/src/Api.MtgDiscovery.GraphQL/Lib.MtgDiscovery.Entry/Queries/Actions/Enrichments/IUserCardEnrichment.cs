using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.DataModels.Entities.Args.User;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Enrichments;

internal interface IUserCardEnrichment
{
    Task Enrich(List<CardItemOutEntity> outEntities, IUserIdArgEntity args, CancellationToken cancellationToken);
    Task EnrichBySet(List<CardItemOutEntity> outEntities, IUserCardsSetItrEntity context, CancellationToken cancellationToken);
    Task EnrichByArtist(List<CardItemOutEntity> outEntities, IUserCardsArtistItrEntity context, CancellationToken cancellationToken);
    Task EnrichByName(List<CardItemOutEntity> outEntities, IUserCardsNameItrEntity context, CancellationToken cancellationToken);
}
