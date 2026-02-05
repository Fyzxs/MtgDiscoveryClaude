using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserWishlistCards;
using Lib.Adapter.UserWishlistCards.Apis.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.UserWishlistCards.Commands;

internal interface IRemoveUserWishlistCardAdapter
{
    Task<IOperationResponse<UserWishlistCardExtEntity>> Execute(IRemoveUserWishlistCardXfrEntity input, CancellationToken cancellationToken);
}
