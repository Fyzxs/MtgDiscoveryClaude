using System.Threading.Tasks;
using Lib.Aggregator.UserWishlistCards.Commands.Entities;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;

namespace Lib.Aggregator.UserWishlistCards.Commands.Mappers;

internal sealed class RemoveUserWishlistCardItrToXfrMapper : IRemoveUserWishlistCardItrToXfrMapper
{
    public Task<RemoveUserWishlistCardXfrEntity> Map(IUserWishlistCardItrEntity source)
    {
        RemoveUserWishlistCardXfrEntity result = new()
        {
            UserId = source.UserId,
            CardId = source.CardId,
            Details = new UserWishlistCardDetailsXfrEntity
            {
                Finish = source.Details.Finish,
                Special = source.Details.Special,
                Count = source.Details.Count
            }
        };
        return Task.FromResult(result);
    }
}
