using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.UserCards.Queries;

internal interface IUserCardsBySetAggregatorService
    : IOperationResponseService<IUserCardsSetItrEntity, IEnumerable<IUserCardOufEntity>>;
