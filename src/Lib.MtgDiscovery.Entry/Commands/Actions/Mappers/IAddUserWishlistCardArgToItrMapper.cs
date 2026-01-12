using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;

namespace Lib.MtgDiscovery.Entry.Commands.Actions.Mappers;

internal interface IAddUserWishlistCardArgToItrMapper : ICreateMapper<IAddCardToWishlistArgsEntity, IUserWishlistCardItrEntity>;
