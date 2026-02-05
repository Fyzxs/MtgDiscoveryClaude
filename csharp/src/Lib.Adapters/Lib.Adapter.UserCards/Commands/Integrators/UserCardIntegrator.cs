using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserCards;
using Lib.Adapter.UserCards.Apis.Entities;

namespace Lib.Adapter.UserCards.Commands.Integrators;

internal sealed class UserCardIntegrator : IUserCardIntegrator
{
    public Task<UserCardExtEntity> Integrate(UserCardExtEntity current, IAddUserCardXfrEntity change)
    {
        ICollection<UserCardDetailsExtEntity> updatedCollectedList = change.ReplaceMode
            ? ReplaceCollectedItem([.. current.CollectedList], change.Details)
            : MergeCollectedItem([.. current.CollectedList], change.Details);

        UserCardExtEntity result = new()
        {
            UserId = current.UserId,
            CardId = current.CardId,
            SetId = current.SetId,
            CardName = change.CardName,
            SetName = change.SetName,
            SetCode = change.SetCode,
            ReleasedAt = change.ReleasedAt,
            Artist = change.Artist,
            ArtistIds = change.ArtistIds,
            CardNameGuid = change.CardNameGuid,
            CollectedList = updatedCollectedList,
            ETag = current.ETag,
            CreatedDate = current.CreatedDate
        };

        return Task.FromResult(result);
    }

    private static ICollection<UserCardDetailsExtEntity> MergeCollectedItem(
        ICollection<UserCardDetailsExtEntity> existingItems,
        IUserCardDetailsXfrEntity newItem)
    {
        Dictionary<(string finish, string special), UserCardDetailsExtEntity> mergedItems = existingItems
            .ToDictionary(item => (item.Finish, item.Special));

        (string finish, string special) key = (newItem.Finish, newItem.Special);

        if (mergedItems.TryGetValue(key, out UserCardDetailsExtEntity existingItem))
        {
            int newCount = existingItem.Count + newItem.Count;
            if (newCount < 0)
            {
                newCount = 0;
            }

            mergedItems[key] = new UserCardDetailsExtEntity
            {
                Finish = existingItem.Finish,
                Special = existingItem.Special,
                Count = newCount
            };
        }
        else
        {
            mergedItems[key] = new UserCardDetailsExtEntity
            {
                Finish = newItem.Finish,
                Special = newItem.Special,
                Count = newItem.Count
            };
        }

        return [.. mergedItems.Values];
    }

    private static ICollection<UserCardDetailsExtEntity> ReplaceCollectedItem(
        ICollection<UserCardDetailsExtEntity> existingItems,
        IUserCardDetailsXfrEntity newItem)
    {
        Dictionary<(string finish, string special), UserCardDetailsExtEntity> items = existingItems
            .ToDictionary(item => (item.Finish, item.Special));

        (string finish, string special) key = (newItem.Finish, newItem.Special);

        items[key] = new UserCardDetailsExtEntity
        {
            Finish = newItem.Finish,
            Special = newItem.Special,
            Count = newItem.Count
        };

        return [.. items.Values];
    }
}
