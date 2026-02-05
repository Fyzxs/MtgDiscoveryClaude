using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserCards;
using Lib.Shared.DataModels.Entities.Args.UserCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.UserCards;

internal interface IUserCardsBySetEntryService
{
    Task<IOperationResponse<List<UserCardOutEntity>>> Execute(IUserCardsBySetArgEntity input, CancellationToken cancellationToken);
}
