using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.UserWishlistCards.Queries;

internal interface IUserWishlistCardsBySetDomainService
{
    Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> Execute(IUserWishlistCardsSetItrEntity input, CancellationToken cancellationToken);
}
