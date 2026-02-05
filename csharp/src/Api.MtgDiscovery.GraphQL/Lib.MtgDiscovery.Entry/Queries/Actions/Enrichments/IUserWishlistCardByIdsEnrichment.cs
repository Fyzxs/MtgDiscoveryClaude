using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.DataModels.Entities.Args.User;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Enrichments;

internal interface IUserWishlistCardByIdsEnrichment
{
    Task Enrich(List<CardItemOutEntity> target, IUserIdArgEntity args, CancellationToken cancellationToken);
}

