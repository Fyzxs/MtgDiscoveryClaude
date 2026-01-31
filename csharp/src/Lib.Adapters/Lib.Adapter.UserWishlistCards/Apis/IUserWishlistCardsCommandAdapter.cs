using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserWishlistCards.Apis.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.UserWishlistCards.Apis;

public interface IUserWishlistCardsCommandAdapter
{
    Task<IOperationResponse<UserWishlistCardExtEntity>> AddUserWishlistCardAsync(IAddUserWishlistCardXfrEntity addUserWishlistCard);
    Task<IOperationResponse<UserWishlistCardExtEntity>> RemoveUserWishlistCardAsync(IRemoveUserWishlistCardXfrEntity removeUserWishlistCard);
}
