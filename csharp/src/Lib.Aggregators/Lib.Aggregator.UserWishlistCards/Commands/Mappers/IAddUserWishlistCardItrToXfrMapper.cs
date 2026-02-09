using Lib.Aggregator.UserWishlistCards.Commands.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;

namespace Lib.Aggregator.UserWishlistCards.Commands.Mappers;

internal interface IAddUserWishlistCardItrToXfrMapper : ICreateMapper<IUserWishlistCardItrEntity, AddUserWishlistCardXfrEntity>;
