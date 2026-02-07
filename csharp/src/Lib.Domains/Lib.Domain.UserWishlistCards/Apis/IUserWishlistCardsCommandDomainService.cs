using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.UserWishlistCards.Apis;

public interface IUserWishlistCardsCommandDomainService
{
    Task<IOperationResponse<IUserWishlistCardOufEntity>> AddUserWishlistCardAsync(IUserWishlistCardItrEntity userWishlistCard, CancellationToken cancellationToken);
    Task<IOperationResponse<IUserWishlistCardOufEntity>> RemoveUserWishlistCardAsync(IUserWishlistCardItrEntity userWishlistCard, CancellationToken cancellationToken);
}
