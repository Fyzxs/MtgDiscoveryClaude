using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Shared.DataModels.Entities.Xfrs.UserSetCards;

namespace Lib.Adapter.UserSetCards.Commands.Integrators;

internal sealed class AddSetGroupIntegrator : IAddSetGroupIntegrator
{
    public Task<UserSetCardExtEntity> Integrate(UserSetCardExtEntity current, IAddSetGroupToUserSetCardXfrEntity change)
    {
        Dictionary<string, UserSetCardCollectingExtEntity> mergedItems = current.Collecting
            .ToDictionary(item => item.SetGroupId);

        string setGroupId = change.SetGroupId;
        FinishCountsExtEntity counts = new()
        {
            Total = change.Counts.Total,
            NonFoil = change.Counts.NonFoil,
            Foil = change.Counts.Foil,
            Etched = change.Counts.Etched
        };

        if (mergedItems.TryGetValue(setGroupId, out UserSetCardCollectingExtEntity existingItem))
        {
            mergedItems[setGroupId] = new UserSetCardCollectingExtEntity
            {
                SetGroupId = setGroupId,
                Collecting = change.Collecting,
                Counts = counts,
                CollectingFinishes = change.CollectingFinishes
            };
        }
        else
        {
            mergedItems[setGroupId] = new UserSetCardCollectingExtEntity
            {
                SetGroupId = setGroupId,
                Collecting = change.Collecting,
                Counts = counts,
                CollectingFinishes = change.CollectingFinishes
            };
        }

        return Task.FromResult(new UserSetCardExtEntity
        {
            UserId = current.UserId,
            SetId = current.SetId,
            TotalCards = current.TotalCards,
            UniqueCards = current.UniqueCards,
            Collecting = [.. mergedItems.Values],
            Groups = current.Groups
        });
    }
}
