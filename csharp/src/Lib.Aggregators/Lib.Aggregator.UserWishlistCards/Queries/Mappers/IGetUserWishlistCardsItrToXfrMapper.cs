using Lib.Aggregator.UserWishlistCards.Queries.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;

namespace Lib.Aggregator.UserWishlistCards.Queries.Mappers;

internal interface IGetUserWishlistCardsItrToXfrMapper : ICreateMapper<IUserWishlistCardsQueryItrEntity, UserWishlistCardsQueryXfrEntity>;
