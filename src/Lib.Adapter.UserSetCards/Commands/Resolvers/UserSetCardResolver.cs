using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Cosmos.Apis.Operators;

namespace Lib.Adapter.UserSetCards.Commands.Resolvers;

internal sealed class UserSetCardResolver : IUserSetCardResolver
{
    public UserSetCardExtEntity Resolve(OpResponse<UserSetCardExtEntity> input, IAddCardToSetXfrEntity context)
    {
        if (input.IsSuccessful()) return input.Value;

        return new UserSetCardExtEntity
        {
            UserId = context.UserId,
            SetId = context.SetId,
            TotalCards = 0,
            UniqueCards = 0,
            Groups = []
        };
    }
}
