using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.Sets;
using Lib.Shared.DataModels.Entities.Args.User;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Enrichments;

internal interface IUserSetEnrichment
{
    Task Enrich(List<SetItemOutEntity> target, IUserIdArgEntity context, CancellationToken cancellationToken);
}
