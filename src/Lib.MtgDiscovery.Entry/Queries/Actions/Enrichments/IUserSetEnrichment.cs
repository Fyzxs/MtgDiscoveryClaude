using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Entities.Outs.Sets;
using Lib.Shared.Abstractions.Actions.Enrichments;
using Lib.Shared.DataModels.Entities.Args.User;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Enrichments;

internal interface IUserSetEnrichment : IEnrichmentAction<List<SetItemOutEntity>, IUserIdArgEntity>
{
}
