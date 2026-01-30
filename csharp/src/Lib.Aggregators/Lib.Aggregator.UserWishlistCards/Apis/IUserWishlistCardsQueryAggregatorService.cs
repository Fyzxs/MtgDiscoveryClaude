using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.UserWishlistCards.Apis;

public interface IUserWishlistCardsQueryAggregatorService
{
    Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> GetUserWishlistCardsAsync(IUserWishlistCardsQueryItrEntity query);

    /// <summary>
    /// Retrieves multiple user wishlist cards using batch point read operations.
    /// </summary>
    /// <param name="userWishlistCards">The user wishlist cards entity containing userId and collection of cardIds</param>
    /// <returns>Collection of found user wishlist cards wrapped in an operation response</returns>
    Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> UserWishlistCardsByIdsAsync(IUserWishlistCardsByIdsItrEntity userWishlistCards);

    /// <summary>
    /// Retrieves user wishlist cards filtered by set.
    /// </summary>
    /// <param name="userWishlistCardsSet">The user wishlist cards set entity containing userId and setId</param>
    /// <returns>Collection of user wishlist cards for the specified set wrapped in an operation response</returns>
    Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> UserWishlistCardsBySetAsync(IUserWishlistCardsSetItrEntity userWishlistCardsSet);
}
