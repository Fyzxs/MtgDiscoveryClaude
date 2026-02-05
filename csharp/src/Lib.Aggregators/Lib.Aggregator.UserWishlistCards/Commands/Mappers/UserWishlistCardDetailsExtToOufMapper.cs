using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserWishlistCards;
using Lib.Aggregator.UserWishlistCards.Entities;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;

namespace Lib.Aggregator.UserWishlistCards.Commands.Mappers;

internal sealed class UserWishlistCardDetailsExtToOufMapper : IUserWishlistCardDetailsExtToOufMapper
{
    public Task<IUserWishlistCardDetailsOufEntity> Map(UserWishlistCardDetailsExtEntity source)
    {
        IUserWishlistCardDetailsOufEntity result = new UserWishlistCardDetailsOufEntity
        {
            Finish = source.Finish,
            Special = source.Special,
            Count = source.Count
        };

        return Task.FromResult(result);
    }
}
