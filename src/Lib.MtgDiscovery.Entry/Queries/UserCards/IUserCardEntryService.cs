using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.DataModels.Entities.Args;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserCards;

namespace Lib.MtgDiscovery.Entry.Queries.UserCards;

internal interface IUserCardEntryService : IOperationResponseService<IUserCardArgEntity, List<UserCardOutEntity>>
{
}
