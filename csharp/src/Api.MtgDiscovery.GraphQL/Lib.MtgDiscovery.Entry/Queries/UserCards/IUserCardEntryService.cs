using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserCards;
using Lib.Shared.DataModels.Entities.Args.UserCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.UserCards;

internal interface IUserCardEntryService
{
    Task<IOperationResponse<List<UserCardOutEntity>>> Execute(IUserCardArgEntity input, CancellationToken cancellationToken);
}
