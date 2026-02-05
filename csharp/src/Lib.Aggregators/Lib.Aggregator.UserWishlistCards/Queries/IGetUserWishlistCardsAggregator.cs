using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.UserWishlistCards.Queries;

internal interface IGetUserWishlistCardsAggregator
{
    Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> Execute(IUserWishlistCardsQueryItrEntity input, CancellationToken cancellationToken);
}
