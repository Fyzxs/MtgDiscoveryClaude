using System.Collections.Generic;
using System.Linq;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Shared.DataModels.Entities.Xfrs.UserSetCards;

namespace Lib.Adapter.UserSetCards.Commands.Integrators;

internal sealed class AddSetGroupIntegrator : IAddSetGroupIntegrator
{
    public UserSetCardExtEntity Integrate(UserSetCardExtEntity current, IAddSetGroupToUserSetCardXfrEntity change)
    {
        Dictionary<string, UserSetCardCollectingExtEntity> mergedItems = current.Collecting
            .ToDictionary(item => item.SetGroupId);

        string setGroupId = change.SetGroupId;

        if (mergedItems.TryGetValue(setGroupId, out UserSetCardCollectingExtEntity? existingItem))
        {
            mergedItems[setGroupId] = new UserSetCardCollectingExtEntity
            {
                SetGroupId = setGroupId,
                Collecting = change.Collecting,
                Count = change.Count
            };
        }
        else
        {
            mergedItems[setGroupId] = new UserSetCardCollectingExtEntity
            {
                SetGroupId = setGroupId,
                Collecting = change.Collecting,
                Count = change.Count
            };
        }

        return new UserSetCardExtEntity
        {
            UserId = current.UserId,
            SetId = current.SetId,
            TotalCards = current.TotalCards,
            UniqueCards = current.UniqueCards,
            Collecting = [.. mergedItems.Values],
            Groups = current.Groups
        };
    }
}
