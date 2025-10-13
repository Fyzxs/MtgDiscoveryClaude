using System.Collections.Generic;
using Lib.Shared.Abstractions.Actions.Enrichments;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;

namespace Lib.MtgDiscovery.Entry.Queries.Enrichments;

internal interface IUserCardBySetEnrichment : IEnrichmentAction<List<CardItemOutEntity>, IUserCardsSetItrEntity>;
