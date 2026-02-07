using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.UserCards.Queries;

/// <summary>
/// Marker interface for retrieving multiple user cards using batch point read operations.
/// Implements single-method delegation pattern with Execute method.
/// </summary>
internal interface IUserCardsByIdsDomainService
{
    Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> Execute(
        IUserCardsByIdsItrEntity input,
        CancellationToken cancellationToken);
}
