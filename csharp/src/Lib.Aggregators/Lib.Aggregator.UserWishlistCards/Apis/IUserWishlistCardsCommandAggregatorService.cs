using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.UserWishlistCards.Apis;

public interface IUserWishlistCardsCommandAggregatorService
{
    Task<IOperationResponse<IUserWishlistCardOufEntity>> AddUserWishlistCardAsync(IUserWishlistCardItrEntity wishlistCard, CancellationToken cancellationToken);
    Task<IOperationResponse<IUserWishlistCardOufEntity>> RemoveUserWishlistCardAsync(IUserWishlistCardItrEntity wishlistCard, CancellationToken cancellationToken);
}
