using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.UserCards.Apis;

public interface IUserCardsAggregatorService
{
    Task<IOperationResponse<IUserCardCollectionItrEntity>> AddUserCardAsync(IUserCardCollectionItrEntity userCard);

    /// <summary>
    /// Retrieves all user cards for a specific user within a given set.
    /// </summary>
    /// <param name="userCardsSet">The user cards set entity containing userId and setId</param>
    /// <returns>Collection of user card collection information wrapped in an operation response</returns>
    Task<IOperationResponse<IEnumerable<IUserCardCollectionItrEntity>>> UserCardsBySetAsync(IUserCardsSetItrEntity userCardsSet);
}
