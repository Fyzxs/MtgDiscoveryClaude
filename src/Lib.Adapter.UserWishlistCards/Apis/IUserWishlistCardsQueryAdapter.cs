using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserWishlistCards.Apis.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.UserWishlistCards.Apis;

public interface IUserWishlistCardsQueryAdapter
{
    /// <summary>
    /// Retrieves all user wishlist cards for a specific user.
    /// </summary>
    /// <param name="userWishlistCard">The user wishlist card entity containing userId</param>
    /// <returns>Collection of user wishlist card information wrapped in an operation response</returns>
    Task<IOperationResponse<IEnumerable<UserWishlistCardExtEntity>>> UserWishlistCardsByUserAsync(IUserWishlistCardXfrEntity userWishlistCard);

    /// <summary>
    /// Retrieves multiple user wishlist cards using parallel point read operations.
    /// </summary>
    /// <param name="userWishlistCards">The user wishlist cards entity containing userId and collection of cardIds</param>
    /// <returns>Collection of found user wishlist cards wrapped in an operation response</returns>
    Task<IOperationResponse<IEnumerable<UserWishlistCardExtEntity>>> UserWishlistCardsByIdsAsync(IUserWishlistCardsByIdsXfrEntity userWishlistCards);
}
