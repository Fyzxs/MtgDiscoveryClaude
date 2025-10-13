using System.Collections.Generic;
using Lib.Shared.Abstractions.Actions.Enrichments;
using Lib.Shared.DataModels.Entities.Args;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;

namespace Lib.MtgDiscovery.Entry.Queries.Enrichments;

internal interface IUserCardByIdsEnrichment : IEnrichmentAction<List<CardItemOutEntity>, IUserIdArgEntity>;
