using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.UserWishlistCards.Apis;

public interface IUserWishlistCardsQueryDomainService
{
    /// <summary>
    /// Retrieves all user wishlist cards for a specific user.
    /// </summary>
    /// <param name="query">The query entity containing userId</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of user wishlist card information wrapped in an operation response</returns>
    Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> GetUserWishlistCardsAsync(IUserWishlistCardsQueryItrEntity query, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all user wishlist cards for a specific user within a given set.
    /// </summary>
    /// <param name="userWishlistCardsSet">The user wishlist cards set entity containing userId and setId</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of user wishlist card information wrapped in an operation response</returns>
    Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> UserWishlistCardsBySetAsync(IUserWishlistCardsSetItrEntity userWishlistCardsSet, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves multiple user wishlist cards using batch point read operations.
    /// </summary>
    /// <param name="userWishlistCards">The user wishlist cards entity containing userId and collection of cardIds</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of found user wishlist cards wrapped in an operation response</returns>
    Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> UserWishlistCardsByIdsAsync(IUserWishlistCardsByIdsItrEntity userWishlistCards, CancellationToken cancellationToken);
}
