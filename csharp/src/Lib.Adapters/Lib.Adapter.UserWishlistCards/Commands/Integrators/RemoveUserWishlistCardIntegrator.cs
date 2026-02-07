using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserWishlistCards;
using Lib.Adapter.UserWishlistCards.Apis.Entities;
using Lib.Universal.Primitives;

namespace Lib.Adapter.UserWishlistCards.Commands.Integrators;

internal sealed class RemoveUserWishlistCardIntegrator : IRemoveUserWishlistCardIntegrator
{
    private readonly TimeInstantString _timestamp;

    public RemoveUserWishlistCardIntegrator()
        : this(new Iso8601UtcNowString())
    { }

    private RemoveUserWishlistCardIntegrator(TimeInstantString timestamp) => _timestamp = timestamp;

    public Task<UserWishlistCardExtEntity> Integrate(
        UserWishlistCardExtEntity current,
        IRemoveUserWishlistCardXfrEntity change)
    {
        ICollection<UserWishlistCardDetailsExtEntity> updatedItems = RemoveWishlistItem(
            [.. current.WishlistItems], change.Details);

        UserWishlistCardExtEntity result = new()
        {
            UserId = current.UserId,
            CardId = current.CardId,
            SetId = current.SetId,
            CardName = current.CardName,
            SetName = current.SetName,
            SetCode = current.SetCode,
            ArtistIds = current.ArtistIds,
            CardNameGuid = current.CardNameGuid,
            WishlistItems = updatedItems,
            CreatedAt = current.CreatedAt,
            UpdatedAt = _timestamp
        };

        return Task.FromResult(result);
    }

    private static ICollection<UserWishlistCardDetailsExtEntity> RemoveWishlistItem(
        ICollection<UserWishlistCardDetailsExtEntity> existingItems,
        IUserWishlistCardDetailsXfrEntity removeItem)
    {
        Dictionary<(string finish, string special), UserWishlistCardDetailsExtEntity> items = existingItems
            .ToDictionary(item => (item.Finish, item.Special));

        (string finish, string special) key = (removeItem.Finish, removeItem.Special);

        if (items.TryGetValue(key, out UserWishlistCardDetailsExtEntity existingItem))
        {
            int newCount = existingItem.Count - removeItem.Count;
            if (0 < newCount)
            {
                items[key] = new UserWishlistCardDetailsExtEntity
                {
                    Finish = existingItem.Finish,
                    Special = existingItem.Special,
                    Count = newCount
                };
            }
            else
            {
                items.Remove(key);
            }
        }

        return [.. items.Values];
    }
}
