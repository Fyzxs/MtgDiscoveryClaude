using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal sealed class UserWishlistCardDetailsOufToOutMapper : IUserWishlistCardDetailsOufToOutMapper
{
    public Task<WishlistItemOutEntity> Map(IUserWishlistCardDetailsOufEntity source)
    {
        WishlistItemOutEntity result = new()
        {
            Finish = source.Finish,
            Special = source.Special,
            Count = source.Count
        };

        return Task.FromResult(result);
    }
}
