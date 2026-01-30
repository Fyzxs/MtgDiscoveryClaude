using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.UserCards.Queries.UserCard;

internal interface IUserCardAggregatorService
    : IOperationResponseService<IUserCardItrEntity, IEnumerable<IUserCardOufEntity>>;
