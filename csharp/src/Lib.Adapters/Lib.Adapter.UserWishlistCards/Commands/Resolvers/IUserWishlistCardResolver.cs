using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserWishlistCards;
using Lib.Adapter.UserWishlistCards.Apis.Entities;
using Lib.Cosmos.Resolvers;

namespace Lib.Adapter.UserWishlistCards.Commands.Resolvers;

internal interface IUserWishlistCardResolver
    : ICosmosResolver<UserWishlistCardExtEntity, IAddUserWishlistCardXfrEntity>;
