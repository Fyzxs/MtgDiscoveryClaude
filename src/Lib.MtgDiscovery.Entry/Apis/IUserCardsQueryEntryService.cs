using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Apis;

/// <summary>
/// Entry service interface for user cards query operations.
/// Separates query operations from command operations following the CQRS pattern.
/// </summary>
public interface IUserCardsQueryEntryService
{
    /// <summary>
    /// Retrieves all user cards for a specific user within a given set.
    /// </summary>
    /// <param name="setArgs">Arguments containing the set ID and user ID to query</param>
    /// <returns>Collection of user card collection information wrapped in an operation response</returns>
    Task<IOperationResponse<IEnumerable<IUserCardCollectionItrEntity>>> UserCardsBySetAsync(IUserCardsSetArgEntity setArgs);
}
