using System;
using System.Collections.Generic;
using System.Linq;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Adapter.UserCards.Apis.Entities;

namespace Lib.Adapter.UserCards.Commands.Mappers;

internal sealed class CollectedItemsMergeMapper : ICollectedItemsMergeMapper
{
    public ICollection<UserCardDetailsExtEntity> Map(ICollection<UserCardDetailsExtEntity> existingItems, IUserCardDetailsXfrEntity newItem)
    {
        // Create a dictionary for efficient merging based on finish + special combination
        Dictionary<(string finish, string special), UserCardDetailsExtEntity> mergedItems = existingItems
            .ToDictionary(item => (item.Finish, item.Special));

        (string finish, string special) key = (newItem.Finish, newItem.Special);

        if (mergedItems.TryGetValue(key, out UserCardDetailsExtEntity existingItem))
        {
            // Update count for existing finish/special combination
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
                // setGroupId from newItem intentionally omitted - used for aggregation only
            };
        }
        else
        {
            // Add new finish/special combination
            mergedItems[key] = new UserCardDetailsExtEntity
            {
                Finish = newItem.Finish,
                Special = newItem.Special,
                Count = newItem.Count
                // setGroupId from newItem intentionally omitted - used for aggregation only
            };
        }

        return [.. mergedItems.Values];
    }
}
