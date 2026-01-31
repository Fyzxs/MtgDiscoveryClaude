using System;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Adapter.UserWishlistCards.Apis.Entities;

namespace Lib.Adapter.UserWishlistCards.Commands.Mappers;

internal sealed class AddUserWishlistCardXfrToExtMapper : IAddUserWishlistCardXfrToExtMapper
{
    public Task<UserWishlistCardExtEntity> Map(IAddUserWishlistCardXfrEntity source)
    {
        IUserWishlistCardDetailsXfrEntity item = source.Details;

        UserWishlistCardDetailsExtEntity wishlistItem = new()
        {
            Finish = item.Finish,
            Special = item.Special,
            Count = item.Count
        };

        string timestamp = DateTime.UtcNow.ToString("o");

        UserWishlistCardExtEntity result = new()
        {
            UserId = source.UserId,
            CardId = source.CardId,
            SetId = source.SetId,
            CardName = source.CardName,
            SetName = source.SetName,
            SetCode = source.SetCode,
            ArtistIds = source.ArtistIds,
            CardNameGuid = source.CardNameGuid,
            WishlistItems = [wishlistItem],
            CreatedAt = timestamp,
            UpdatedAt = timestamp
        };

        return Task.FromResult(result);
    }
}
