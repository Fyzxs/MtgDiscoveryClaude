using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.DataModels.Entities.Oufs.UserSetCards;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.UserSetCards.Queries;

internal interface IAllUserSetCardsAggregatorService
    : IOperationResponseService<IAllUserSetCardsItrEntity, IEnumerable<IUserSetCardOufEntity>>;
