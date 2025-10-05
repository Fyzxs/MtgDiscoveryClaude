using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Adapter.UserSetCards.Apis.Entities;

namespace Lib.Adapter.UserSetCards.Commands.Mappers;

internal sealed class UserSetCardsUpsertXfrToExtMapper : IUserSetCardsUpsertXfrToExtMapper
{
    public Task<UserSetCardExtEntity> Map(IUserSetCardUpsertXfrEntity source)
    {
        UserSetCardExtEntity extEntity = new()
        {
            UserId = source.UserId,
            SetId = source.SetId,
            TotalCards = source.TotalCards,
            UniqueCards = source.UniqueCards,
            Groups = MapGroups(source.Groups)
        };

        return Task.FromResult(extEntity);
    }

    private static Dictionary<string, UserSetCardGroupExtEntity> MapGroups(IDictionary<string, IUserSetCardGroupXfrEntity> xfrGroups)
    {
        Dictionary<string, UserSetCardGroupExtEntity> extGroups = [];

        foreach (KeyValuePair<string, IUserSetCardGroupXfrEntity> kvp in xfrGroups)
        {
            extGroups[kvp.Key] = MapGroup(kvp.Value);
        }

        return extGroups;
    }

    private static UserSetCardGroupExtEntity MapGroup(IUserSetCardGroupXfrEntity xfrGroup)
    {
        return new UserSetCardGroupExtEntity
        {
            NonFoil = MapFinishGroup(xfrGroup.NonFoil),
            Foil = MapFinishGroup(xfrGroup.Foil),
            Etched = MapFinishGroup(xfrGroup.Etched)
        };
    }

    private static UserSetCardFinishGroupExtEntity MapFinishGroup(IUserSetCardFinishGroupXfrEntity xfrFinishGroup)
    {
        return new UserSetCardFinishGroupExtEntity
        {
            Cards = xfrFinishGroup.Cards
        };
    }
}
