using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.UserWishlistCards.Queries.UserWishlistCardsByIds;

internal interface IUserWishlistCardsByIdsAggregatorService
    : IOperationResponseService<IUserWishlistCardsByIdsItrEntity, IEnumerable<IUserWishlistCardOufEntity>>;
