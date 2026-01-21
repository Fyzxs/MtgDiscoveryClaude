using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.Abstractions.Actions.Enrichments;
using Lib.Shared.DataModels.Entities.Args.User;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Enrichments;

internal interface IUserWishlistCardByIdsEnrichment : IEnrichmentAction<List<CardItemOutEntity>, IUserIdArgEntity>;

