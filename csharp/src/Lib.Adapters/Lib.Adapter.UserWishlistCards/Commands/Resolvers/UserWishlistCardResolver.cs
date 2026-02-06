using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserWishlistCards;
using Lib.Adapter.UserWishlistCards.Apis.Entities;
using Lib.Cosmos.Apis.Operators;
using Lib.Universal.Primitives;

namespace Lib.Adapter.UserWishlistCards.Commands.Resolvers;

internal sealed class UserWishlistCardResolver : IUserWishlistCardResolver
{
    private readonly TimeInstantString _timestamp;

    public UserWishlistCardResolver()
        : this(new Iso8601UtcNowString())
    { }

    private UserWishlistCardResolver(TimeInstantString timestamp) => _timestamp = timestamp;

    public UserWishlistCardExtEntity Resolve(OpResponse<UserWishlistCardExtEntity> input, IAddUserWishlistCardXfrEntity context)
    {
        if (input.IsSuccessful())
            return input.Value;

        return new UserWishlistCardExtEntity
        {
            UserId = context.UserId,
            CardId = context.CardId,
            SetId = context.SetId,
            CardName = context.CardName,
            SetName = context.SetName,
            SetCode = context.SetCode,
            ArtistIds = context.ArtistIds,
            CardNameGuid = context.CardNameGuid,
            WishlistItems = [],
            CreatedAt = _timestamp,
            UpdatedAt = _timestamp
        };
    }
}
