using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserCards;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.Invocation.Services;

namespace Lib.MtgDiscovery.Entry.Queries.UserCards;

internal interface IUserCardsBySetEntryService : IOperationResponseService<IUserCardsBySetArgEntity, List<UserCardOutEntity>>
{
}
