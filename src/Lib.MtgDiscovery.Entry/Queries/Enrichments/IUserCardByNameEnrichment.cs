using System.Collections.Generic;
using Lib.Shared.Abstractions.Actions.Enrichments;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.DataModels.Entities.Outs.Cards;

namespace Lib.MtgDiscovery.Entry.Queries.Enrichments;

internal interface IUserCardByNameEnrichment : IEnrichmentAction<List<CardItemOutEntity>, IUserCardsNameItrEntity>;
