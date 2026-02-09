using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;
using Lib.Shared.Invocation.Services;

namespace Lib.Domain.UserWishlistCards.Queries;

internal interface IUserWishlistCardsBySetDomain
    : IOperationResponseService<IUserWishlistCardsSetItrEntity, IEnumerable<IUserWishlistCardOufEntity>>;
