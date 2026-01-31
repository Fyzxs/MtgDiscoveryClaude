using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;
using Lib.Shared.Invocation.Services;

namespace Lib.Domain.UserWishlistCards.Commands;

internal interface IRemoveUserWishlistCardDomainService
    : IOperationResponseService<IUserWishlistCardItrEntity, IUserWishlistCardOufEntity>;
