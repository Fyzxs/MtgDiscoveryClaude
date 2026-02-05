using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.UserWishlistCards.Commands;

internal interface IRemoveUserWishlistCardDomainService
{
    Task<IOperationResponse<IUserWishlistCardOufEntity>> Execute(IUserWishlistCardItrEntity input, CancellationToken cancellationToken);
}
