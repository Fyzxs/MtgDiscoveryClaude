using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserWishlistCards;
using Lib.Adapter.UserWishlistCards.Apis.Entities;

namespace Lib.Adapter.UserWishlistCards.Commands.Integrators;

internal sealed class UserWishlistCardIntegrator : IUserWishlistCardIntegrator
{
    public Task<UserWishlistCardExtEntity> Integrate(
        UserWishlistCardExtEntity current,
        IAddUserWishlistCardXfrEntity change)
    {
        ICollection<UserWishlistCardDetailsExtEntity> updatedItems = MergeWishlistItem(
            [.. current.WishlistItems], change.Details);

        UserWishlistCardExtEntity result = new()
        {
            UserId = current.UserId,
            CardId = current.CardId,
            SetId = current.SetId,
            CardName = change.CardName,
            SetName = change.SetName,
            SetCode = change.SetCode,
            ArtistIds = change.ArtistIds,
            CardNameGuid = change.CardNameGuid,
            WishlistItems = updatedItems,
            CreatedAt = current.CreatedAt,
            UpdatedAt = DateTime.UtcNow.ToString("o")
        };

        return Task.FromResult(result);
    }

    private static ICollection<UserWishlistCardDetailsExtEntity> MergeWishlistItem(
        ICollection<UserWishlistCardDetailsExtEntity> existingItems,
        IUserWishlistCardDetailsXfrEntity newItem)
    {
        Dictionary<(string finish, string special), UserWishlistCardDetailsExtEntity> mergedItems = existingItems
            .ToDictionary(item => (item.Finish, item.Special));

        (string finish, string special) key = (newItem.Finish, newItem.Special);

        if (mergedItems.TryGetValue(key, out UserWishlistCardDetailsExtEntity existingItem))
        {
            int newCount = existingItem.Count + newItem.Count;
            if (0 < newCount)
            {
                mergedItems[key] = new UserWishlistCardDetailsExtEntity
                {
                    Finish = existingItem.Finish,
                    Special = existingItem.Special,
                    Count = newCount
                };
            }
            else
            {
                mergedItems.Remove(key);
            }
        }
        else
        {
            if (0 < newItem.Count)
            {
                mergedItems[key] = new UserWishlistCardDetailsExtEntity
                {
                    Finish = newItem.Finish,
                    Special = newItem.Special,
                    Count = newItem.Count
                };
            }
        }

        return [.. mergedItems.Values];
    }
}
