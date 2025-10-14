using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserCards;
using Lib.Shared.DataModels.Entities.Args;

namespace Lib.MtgDiscovery.Entry.Queries.UserCards;

internal interface IUserCardsByIdsEntryService : IOperationResponseService<IUserCardsByIdsArgEntity, List<UserCardOutEntity>>
{
}
