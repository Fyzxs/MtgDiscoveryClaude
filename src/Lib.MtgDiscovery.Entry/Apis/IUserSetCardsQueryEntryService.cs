using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Outs.UserSetCards;
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
    Task<IOperationResponse<UserSetCardOutEntity>> GetUserSetCardByUserAndSetAsync(IUserSetCardArgEntity userSetCardArgs);
}
