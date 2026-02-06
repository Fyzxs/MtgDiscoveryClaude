using System;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserWishlistCards;
using Lib.Adapter.UserWishlistCards.Apis.Entities;
using Lib.Cosmos.Apis.Operators;

namespace Lib.Adapter.UserWishlistCards.Commands.Resolvers;

internal sealed class UserWishlistCardResolver : IUserWishlistCardResolver
{
    public UserWishlistCardExtEntity Resolve(OpResponse<UserWishlistCardExtEntity> input, IAddUserWishlistCardXfrEntity context)
    {
        if (input.IsSuccessful())
            return input.Value;

        string timestamp = DateTime.UtcNow.ToString("o");

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
            CreatedAt = timestamp,
            UpdatedAt = timestamp
        };
    }
}
