using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserWishlistCards;
using Lib.Adapter.UserWishlistCards.Apis.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;

namespace Lib.Adapter.UserWishlistCards.Commands.Mappers;

internal interface IAddUserWishlistCardXfrToExtMapper : ICreateMapper<IAddUserWishlistCardXfrEntity, UserWishlistCardExtEntity>;
