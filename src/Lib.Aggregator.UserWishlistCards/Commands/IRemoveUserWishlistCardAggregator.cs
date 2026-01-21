using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.UserWishlistCards.Commands;

internal interface IRemoveUserWishlistCardAggregator
    : IOperationResponseService<IUserWishlistCardItrEntity, IUserWishlistCardOufEntity>;
