using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSetCards;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Apis;

/// <summary>
/// Entry service interface for user set cards query operations.
/// </summary>
public interface IUserSetCardsQueryEntryService
{
    /// <summary>
    /// Retrieves user set card collection summary for a specific user and set.
    /// </summary>
    /// <param name="userSetCardArgs">Arguments containing the user ID and set ID to query</param>
    /// <returns>User set card collection summary wrapped in an operation response</returns>
    Task<IOperationResponse<UserSetCardOutEntity>> UserSetCardByUserAndSetAsync(IUserSetCardArgEntity userSetCardArgs);

    /// <summary>
    /// Retrieves all user set card collection summaries for a specific user.
    /// </summary>
    /// <param name="userSetCardsArgs">Arguments containing the user ID to query</param>
    /// <returns>Collection of user set card summaries wrapped in an operation response</returns>
    Task<IOperationResponse<List<UserSetCardOutEntity>>> AllUserSetCardsAsync(IAllUserSetCardsArgEntity userSetCardsArgs);
}
