using System.Collections.Generic;
using System.Linq;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Adapter.UserCards.Apis.Entities;

namespace Lib.Adapter.UserCards.Commands.Mappers;

internal sealed class CollectedItemsReplaceMapper : ICollectedItemsReplaceMapper
{
    public ICollection<UserCardDetailsExtEntity> Map(ICollection<UserCardDetailsExtEntity> existingItems, IUserCardDetailsXfrEntity newItem)
    {
        // Create a dictionary and replace (or add) the item for this finish/special combination
        Dictionary<(string finish, string special), UserCardDetailsExtEntity> items = existingItems
            .ToDictionary(item => (item.Finish, item.Special));

        (string finish, string special) key = (newItem.Finish, newItem.Special);

        // Replace the count directly (don't add to existing)
        items[key] = new UserCardDetailsExtEntity
        {
            Finish = newItem.Finish,
            Special = newItem.Special,
            Count = newItem.Count
        };

        return [.. items.Values];
    }
}
