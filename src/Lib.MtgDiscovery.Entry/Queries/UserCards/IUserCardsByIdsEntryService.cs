using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Outs.UserCards;

namespace Lib.MtgDiscovery.Entry.Queries.UserCards;

internal interface IUserCardsByIdsEntryService : IOperationResponseService<IUserCardsByIdsArgEntity, List<UserCardOutEntity>>
{
}
