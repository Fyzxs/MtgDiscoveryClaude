using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Adapter.UserSetCards.Commands.Resolvers;

namespace Lib.Adapter.UserSetCards.Commands.Integrators;

internal sealed class UserSetCardIntegrator : IUserSetCardIntegrator
{
    private readonly IUserSetCardGroupResolver _groupResolver;

    public UserSetCardIntegrator() : this(new UserSetCardGroupResolver())
    { }

    private UserSetCardIntegrator(IUserSetCardGroupResolver groupResolver) => _groupResolver = groupResolver;

    public Task<UserSetCardExtEntity> Integrate(UserSetCardExtEntity current, IAddCardToSetXfrEntity change)
    {
        if (change.Count == 0) return Task.FromResult(current);

        return Task.FromResult(ApplyCardCountChange(current, change));
    }

    private UserSetCardExtEntity ApplyCardCountChange(UserSetCardExtEntity record, IAddCardToSetXfrEntity entity)
    {
        Dictionary<string, UserSetCardFinishGroupExtEntity> finishGroups = _groupResolver.Resolve(record, entity);
        int uniqueCardDelta = UpdateCardToGroup(finishGroups, entity);

        return new UserSetCardExtEntity
        {
            UserId = record.UserId,
            SetId = record.SetId,
            TotalCards = Math.Max(0, record.TotalCards + entity.Count),
            UniqueCards = Math.Max(0, record.UniqueCards + uniqueCardDelta),
            Groups = record.Groups,
            Collecting = UpdateCollectingCounts(record.Collecting, entity, uniqueCardDelta),
            ETag = record.ETag,  // Preserve ETag for optimistic concurrency
            CreatedDate = record.CreatedDate  // Preserve creation date
        };
    }

    private ICollection<UserSetCardCollectingExtEntity> UpdateCollectingCounts(
        ICollection<UserSetCardCollectingExtEntity> collecting,
        IAddCardToSetXfrEntity entity,
        int uniqueCardDelta)
    {
        if (collecting == null || collecting.Count == 0) return collecting;

        return [.. collecting.Select(c => UpdateCollectingEntry(c, entity, uniqueCardDelta))];
    }

    private UserSetCardCollectingExtEntity UpdateCollectingEntry(
        UserSetCardCollectingExtEntity entry,
        IAddCardToSetXfrEntity entity,
        int uniqueCardDelta)
    {
        if (entry.SetGroupId != entity.SetGroupId) return entry;
        if (entry.Collecting is false) return entry;

        string finishTypeLower = entity.FinishType.ToLowerInvariant();
        FinishCountsExtEntity currentCounts = entry.Counts ?? new FinishCountsExtEntity();

        int nonFoilDelta = finishTypeLower == "nonfoil" ? uniqueCardDelta : 0;
        int foilDelta = finishTypeLower == "foil" ? uniqueCardDelta : 0;
        int etchedDelta = finishTypeLower == "etched" ? uniqueCardDelta : 0;

        FinishCountsExtEntity newCounts = new()
        {
            NonFoil = Math.Max(0, currentCounts.NonFoil + nonFoilDelta),
            Foil = Math.Max(0, currentCounts.Foil + foilDelta),
            Etched = Math.Max(0, currentCounts.Etched + etchedDelta),
            Total = Math.Max(0, currentCounts.Total + uniqueCardDelta)
        };

        return new UserSetCardCollectingExtEntity
        {
            SetGroupId = entry.SetGroupId,
            Collecting = entry.Collecting,
            Count = entry.Count,
            Counts = newCounts,
            CollectingFinishes = entry.CollectingFinishes
        };
    }

    private int UpdateCardToGroup(Dictionary<string, UserSetCardFinishGroupExtEntity> finishGroups, IAddCardToSetXfrEntity entity)
    {
        bool isAdding = 0 < entity.Count;
        string finishTypeLower = entity.FinishType.ToLowerInvariant();
        UserSetCardFinishGroupExtEntity toModify = finishGroups[finishTypeLower];
        if (toModify.Cards.Contains(entity.CardId) == isAdding) return 0;

        if (isAdding)
        {
            toModify.Cards.Add(entity.CardId);
            return 1;
        }

        if (0 < entity.RemainingFinishCount) { return 0; }

        _ = toModify.Cards.Remove(entity.CardId);
        return -1;
    }
}
