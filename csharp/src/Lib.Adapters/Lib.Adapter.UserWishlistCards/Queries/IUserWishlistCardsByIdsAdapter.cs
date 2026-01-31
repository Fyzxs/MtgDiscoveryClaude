using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserWishlistCards.Apis.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.UserWishlistCards.Queries;

internal interface IUserWishlistCardsByIdsAdapter
{
    Task<IOperationResponse<IEnumerable<UserWishlistCardExtEntity>>> Execute(IUserWishlistCardsByIdsXfrEntity input);
}
