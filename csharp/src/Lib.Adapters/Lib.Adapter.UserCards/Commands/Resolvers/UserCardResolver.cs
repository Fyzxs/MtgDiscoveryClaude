using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Cosmos.Apis.Operators;

namespace Lib.Adapter.UserCards.Commands.Resolvers;

internal sealed class UserCardResolver : IUserCardResolver
{
    public UserCardExtEntity Resolve(OpResponse<UserCardExtEntity> input, IAddUserCardXfrEntity context)
    {
        if (input.IsSuccessful())
            return input.Value;

        return new UserCardExtEntity
        {
            UserId = context.UserId,
            CardId = context.CardId,
            SetId = context.SetId,
            CardName = context.CardName,
            SetName = context.SetName,
            SetCode = context.SetCode,
            ReleasedAt = context.ReleasedAt,
            Artist = context.Artist,
            ArtistIds = context.ArtistIds,
            CardNameGuid = context.CardNameGuid,
            CollectedList = []
        };
    }
}
