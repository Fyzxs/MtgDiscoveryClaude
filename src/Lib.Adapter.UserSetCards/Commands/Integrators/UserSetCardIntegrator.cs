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

    public UserSetCardExtEntity Integrate(UserSetCardExtEntity current, IAddCardToSetXfrEntity change)
    {
        if (change.Count == 0) return current;

        return ApplyCardCountChange(current, change);
    }

    private UserSetCardExtEntity ApplyCardCountChange(UserSetCardExtEntity record, IAddCardToSetXfrEntity entity)
    {
        Dictionary<string, UserSetCardFinishGroupExtEntity> finishGroups = _groupResolver.Resolve(record, entity);

        return new UserSetCardExtEntity
        {
            UserId = record.UserId,
            SetId = record.SetId,
            TotalCards = Math.Max(0, record.TotalCards + entity.Count),
            UniqueCards = Math.Max(0, record.UniqueCards + UpdateCardToGroup(finishGroups, entity)),
            Collecting = record.Collecting,
            Groups = record.Groups
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

        _ = toModify.Cards.Remove(entity.CardId);
        return -1;
    }
}
