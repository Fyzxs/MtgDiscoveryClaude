using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.Abstractions.Actions.Enrichments;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Enrichments;

internal interface IUserCardByNameEnrichment : IEnrichmentAction<List<CardItemOutEntity>, IUserCardsNameItrEntity>;
