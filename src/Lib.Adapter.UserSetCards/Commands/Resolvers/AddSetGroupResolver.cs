using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.DataModels.Entities.Xfrs.UserSetCards;

namespace Lib.Adapter.UserSetCards.Commands.Resolvers;

internal sealed class AddSetGroupResolver : IAddSetGroupResolver
{
    public UserSetCardExtEntity Resolve(OpResponse<UserSetCardExtEntity> input, IAddSetGroupToUserSetCardXfrEntity context)
    {
        if (input.IsSuccessful()) return input.Value;

        return new UserSetCardExtEntity
        {
            UserId = context.UserId,
            SetId = context.SetId,
            TotalCards = 0,
            UniqueCards = 0,
            Collecting = [],
            Groups = []
        };
    }
}
