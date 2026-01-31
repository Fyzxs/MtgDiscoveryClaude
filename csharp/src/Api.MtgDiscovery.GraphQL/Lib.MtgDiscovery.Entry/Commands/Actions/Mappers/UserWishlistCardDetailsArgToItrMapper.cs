using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Commands.Entities;
using Lib.Shared.DataModels.Entities.Args.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;

namespace Lib.MtgDiscovery.Entry.Commands.Actions.Mappers;

internal sealed class UserWishlistCardDetailsArgToItrMapper : IUserWishlistCardDetailsArgToItrMapper
{
    public Task<IUserWishlistCardDetailsItrEntity> Map(IUserWishlistCardDetailsArgEntity argItem)
    {
        return Task.FromResult<IUserWishlistCardDetailsItrEntity>(new UserWishlistCardDetailsItrEntity
        {
            Finish = argItem.Finish,
            Special = argItem.Special,
            Count = argItem.Count
        });
    }
}
