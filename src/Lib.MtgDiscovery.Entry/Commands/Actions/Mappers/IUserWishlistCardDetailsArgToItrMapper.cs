using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Args.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;

namespace Lib.MtgDiscovery.Entry.Commands.Actions.Mappers;

internal interface IUserWishlistCardDetailsArgToItrMapper : ICreateMapper<IUserWishlistCardDetailsArgEntity, IUserWishlistCardDetailsItrEntity>;
