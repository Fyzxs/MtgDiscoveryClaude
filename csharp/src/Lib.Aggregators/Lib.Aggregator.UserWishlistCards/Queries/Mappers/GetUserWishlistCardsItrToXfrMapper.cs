using System.Threading.Tasks;
using Lib.Aggregator.UserWishlistCards.Queries.Entities;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;

namespace Lib.Aggregator.UserWishlistCards.Queries.Mappers;

internal sealed class GetUserWishlistCardsItrToXfrMapper : IGetUserWishlistCardsItrToXfrMapper
{
    public Task<UserWishlistCardsQueryXfrEntity> Map(IUserWishlistCardsQueryItrEntity source)
    {
        UserWishlistCardsQueryXfrEntity result = new()
        {
            UserId = source.UserId
        };
        return Task.FromResult(result);
    }
}
