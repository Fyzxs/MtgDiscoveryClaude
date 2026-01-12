using System.Threading.Tasks;
using Lib.Adapter.UserWishlistCards.Apis.Entities;
using Lib.Aggregator.UserWishlistCards.Queries.Entities;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;

namespace Lib.Aggregator.UserWishlistCards.Queries.Mappers;

internal sealed class UserWishlistCardsByIdsItrToXfrMapper : IUserWishlistCardsByIdsItrToXfrMapper
{
    public Task<IUserWishlistCardsByIdsXfrEntity> Map(IUserWishlistCardsByIdsItrEntity source)
    {
        return Task.FromResult<IUserWishlistCardsByIdsXfrEntity>(new UserWishlistCardsByIdsXfrEntity
        {
            UserId = source.UserId,
            CardIds = source.CardIds
        });
    }
}
