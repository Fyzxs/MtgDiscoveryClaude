using System.Collections.Generic;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserWishlistCards.Apis.Entities;
using Lib.Shared.Invocation.Services;

namespace Lib.Adapter.UserWishlistCards.Queries;

internal interface IUserWishlistCardsByUserAdapter
    : IOperationResponseService<IUserWishlistCardXfrEntity, IEnumerable<UserWishlistCardExtEntity>>;
