using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;

namespace Lib.MtgDiscovery.Entry.Commands.Actions.Mappers;

internal interface IAddUserWishlistCardArgToItrMapper //TODO: Implement the interface all other mappers implement
{
    Task<IUserWishlistCardItrEntity> Map(IAddCardToWishlistArgsEntity source);
}
