using Lib.Adapter.UserWishlistCards.Apis.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;

namespace Lib.Aggregator.UserWishlistCards.Queries.Mappers;

internal interface IUserWishlistCardsByIdsItrToXfrMapper : ICreateMapper<IUserWishlistCardsByIdsItrEntity, IUserWishlistCardsByIdsXfrEntity>;
