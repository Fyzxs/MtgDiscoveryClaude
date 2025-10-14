using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.Abstractions.Actions.Enrichments;
using Lib.Shared.DataModels.Entities.Args;

namespace Lib.MtgDiscovery.Entry.Queries.Enrichments;

internal interface IUserCardByIdsEnrichment : IEnrichmentAction<List<CardItemOutEntity>, IUserIdArgEntity>;
