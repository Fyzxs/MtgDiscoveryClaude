using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.UserWishlistCards.Queries.UserWishlistCardsByIds;

internal interface IUserWishlistCardsByIdsAggregatorService
{
    Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> Execute(IUserWishlistCardsByIdsItrEntity input);
}
