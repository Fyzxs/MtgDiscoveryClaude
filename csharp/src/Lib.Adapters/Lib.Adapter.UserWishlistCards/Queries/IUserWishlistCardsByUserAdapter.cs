using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserWishlistCards.Apis.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.UserWishlistCards.Queries;

internal interface IUserWishlistCardsByUserAdapter
{
    Task<IOperationResponse<IEnumerable<UserWishlistCardExtEntity>>> Execute(IUserWishlistCardXfrEntity input, CancellationToken cancellationToken);
}
