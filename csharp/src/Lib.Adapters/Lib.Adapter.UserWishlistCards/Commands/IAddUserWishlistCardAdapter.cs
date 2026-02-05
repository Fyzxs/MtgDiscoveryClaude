using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserWishlistCards.Apis.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.UserWishlistCards.Commands;

internal interface IAddUserWishlistCardAdapter
{
    Task<IOperationResponse<UserWishlistCardExtEntity>> Execute(IAddUserWishlistCardXfrEntity input, CancellationToken cancellationToken);
}
