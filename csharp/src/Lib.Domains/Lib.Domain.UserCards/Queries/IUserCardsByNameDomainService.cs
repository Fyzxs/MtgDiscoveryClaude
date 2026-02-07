using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.UserCards.Queries;

/// <summary>
/// Marker interface for retrieving all user cards with a specific card name.
/// Implements single-method delegation pattern with Execute method.
/// </summary>
internal interface IUserCardsByNameDomainService
{
    Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> Execute(
        IUserCardsNameItrEntity input,
        CancellationToken cancellationToken);
}
