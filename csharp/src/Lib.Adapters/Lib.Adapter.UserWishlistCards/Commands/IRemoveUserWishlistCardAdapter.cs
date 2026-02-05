using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserWishlistCards;
using Lib.Adapter.UserWishlistCards.Apis.Entities;
using Lib.Shared.Invocation.Services;

namespace Lib.Adapter.UserWishlistCards.Commands;

internal interface IRemoveUserWishlistCardAdapter
    : IOperationResponseService<IRemoveUserWishlistCardXfrEntity, UserWishlistCardExtEntity>;
