using System.Collections.Generic;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Adapter.UserSetCards.Apis.Entities;

namespace Lib.Adapter.UserSetCards.Commands.Resolvers;

internal sealed class UserSetCardGroupResolver : IUserSetCardGroupResolver
{
    public Dictionary<string, UserSetCardFinishGroupExtEntity> Resolve(UserSetCardExtEntity input, IAddCardToSetXfrEntity context)
    {
        if (input.Groups.TryGetValue(context.SetGroupId, out UserSetCardGroupExtEntity? group))
        {
            return new Dictionary<string, UserSetCardFinishGroupExtEntity>
            {
                {"foil", group.Foil},
                {"nonfoil", group.NonFoil},
                {"etched", group.Etched}
            };
        }

        return new Dictionary<string, UserSetCardFinishGroupExtEntity>
        {
            {"foil", new UserSetCardFinishGroupExtEntity { Cards = [] }},
            {"nonfoil", new UserSetCardFinishGroupExtEntity { Cards = [] }},
            {"etched", new UserSetCardFinishGroupExtEntity { Cards = [] }}
        };
    }
}
