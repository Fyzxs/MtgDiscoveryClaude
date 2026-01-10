using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Args.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;

namespace Lib.MtgDiscovery.Entry.Commands.Actions.Mappers;

internal interface IUserWishlistCardDetailsArgToItrMapper //TODO: Implement the interface all other mappers implement
{
    Task<IUserWishlistCardDetailsItrEntity> Map(IUserWishlistCardDetailsArgEntity argItem);
}
