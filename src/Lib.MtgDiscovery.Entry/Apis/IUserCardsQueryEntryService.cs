using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserCards;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Apis;

/// <summary>
/// Entry service interface for user cards query operations.
/// Separates query operations from command operations following the CQRS pattern.
/// </summary>
public interface IUserCardsQueryEntryService
{
    /// <summary>
    /// Retrieves a specific user card using point read operation.
    /// </summary>
    /// <param name="cardArgs">Arguments containing the user ID and card ID to query</param>
    /// <returns>Collection containing zero or one user card wrapped in an operation response</returns>
    Task<IOperationResponse<List<UserCardOutEntity>>> UserCardAsync(IUserCardArgEntity cardArgs);

    /// <summary>
    /// Retrieves all user cards for a specific user within a given set.
    /// </summary>
    /// <param name="bySetArgs">Arguments containing the set ID and user ID to query</param>
    /// <returns>Collection of user card collection information wrapped in an operation response</returns>
    Task<IOperationResponse<List<UserCardOutEntity>>> UserCardsBySetAsync(IUserCardsBySetArgEntity bySetArgs);

    /// <summary>
    /// Retrieves multiple user cards by their IDs using batch point read operation.
    /// </summary>
    /// <param name="cardsArgs">Arguments containing the user ID and card IDs to query</param>
    /// <returns>Collection of user cards wrapped in an operation response</returns>
    Task<IOperationResponse<List<UserCardOutEntity>>> UserCardsByIdsAsync(IUserCardsByIdsArgEntity cardsArgs);
}
