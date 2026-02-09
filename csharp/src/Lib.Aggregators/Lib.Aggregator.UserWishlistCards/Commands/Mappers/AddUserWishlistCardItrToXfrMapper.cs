using System.Threading.Tasks;
using Lib.Aggregator.UserWishlistCards.Commands.Entities;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;

namespace Lib.Aggregator.UserWishlistCards.Commands.Mappers;

internal sealed class AddUserWishlistCardItrToXfrMapper : IAddUserWishlistCardItrToXfrMapper
{
    public Task<AddUserWishlistCardXfrEntity> Map(IUserWishlistCardItrEntity source)
    {
        AddUserWishlistCardXfrEntity result = new()
        {
            UserId = source.UserId,
            CardId = source.CardId,
            SetId = source.SetId,
            CardName = source.CardName,
            SetName = source.SetName,
            SetCode = source.SetCode,
            ArtistIds = source.ArtistIds,
            CardNameGuid = source.CardNameGuid,
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
