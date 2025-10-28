using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.UserCards.Queries.UserCardsByName;

internal interface IUserCardsByNameAggregatorService
    : IOperationResponseService<IUserCardsNameItrEntity, IEnumerable<IUserCardOufEntity>>;
