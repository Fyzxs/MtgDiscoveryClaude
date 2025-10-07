using System.Threading.Tasks;
using Lib.Aggregator.UserSetCards.Entities;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.UserSetCards.Apis;

public interface IUserSetCardsDomainService
{
    /// <summary>
    /// Retrieves user set card collection summary for a specific user and set.
    /// </summary>
    /// <param name="userSetCard">The user set card entity containing userId and setId</param>
    /// <returns>User set card collection summary wrapped in an operation response</returns>
    Task<IOperationResponse<IUserSetCardOufEntity>> GetUserSetCardByUserAndSetAsync(IUserSetCardItrEntity userSetCard);

    Task<IOperationResponse<IUserSetCardOufEntity>> AddSetGroupToUserSetCardAsync(IAddSetGroupToUserSetCardItrEntity entity);
}
