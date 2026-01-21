using Lib.MtgDiscovery.Entry.Entities.Outs.UserWishlistCards;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal interface IUserWishlistCardOufToOutMapper : ICreateMapper<IUserWishlistCardOufEntity, UserWishlistCardOutEntity>;

