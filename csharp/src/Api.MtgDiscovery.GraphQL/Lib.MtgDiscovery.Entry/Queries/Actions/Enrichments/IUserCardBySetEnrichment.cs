using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Enrichments;

internal interface IUserCardBySetEnrichment
{
    Task Enrich(List<CardItemOutEntity> target, IUserCardsSetItrEntity context, CancellationToken cancellationToken);
}
