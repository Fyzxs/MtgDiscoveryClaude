using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards.Signing;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.UserCards.Apis;

public interface IUserCardsQueryDomainService
{
    /// <summary>
    /// Retrieves a specific user card using point read operation.
    /// </summary>
    /// <param name="userCard">The user card entity containing userId and cardId</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>Collection containing zero or one user card wrapped in an operation response</returns>
    Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardAsync(
        IUserCardItrEntity userCard,
        CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all user cards for a specific user within a given set.
    /// </summary>
    /// <param name="userCardsSet">The user cards set entity containing userId and setId</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>Collection of user card collection information wrapped in an operation response</returns>
    Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsBySetAsync(
        IUserCardsSetItrEntity userCardsSet,
        CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves multiple user cards using batch point read operations.
    /// </summary>
    /// <param name="userCards">The user cards entity containing userId and collection of cardIds</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>Collection of found user cards wrapped in an operation response</returns>
    Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByIdsAsync(
        IUserCardsByIdsItrEntity userCards,
        CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all user cards for a specific artist.
    /// </summary>
    /// <param name="userCardsArtist">The user cards artist entity containing userId and artistId</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>Collection of user cards for the artist wrapped in an operation response</returns>
    Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByArtistAsync(
        IUserCardsArtistItrEntity userCardsArtist,
        CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all user cards with a specific card name.
    /// </summary>
    /// <param name="userCardsName">The user cards name entity containing userId and cardName</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>Collection of user cards with the name wrapped in an operation response</returns>
    Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByNameAsync(
        IUserCardsNameItrEntity userCardsName,
        CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves user cards for multiple artists grouped by set for convention signing planning.
    /// </summary>
    /// <param name="userCardsForSigning">The entity containing userId and collection of artistIds</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>Signing result grouped by set and artist wrapped in an operation response</returns>
    Task<IOperationResponse<ISigningResultOufEntity>> UserCardsForSigningAsync(
        IUserCardsForSigningItrEntity userCardsForSigning,
        CancellationToken cancellationToken);
}
