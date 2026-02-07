using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards.Signing;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.UserCards.Queries;

/// <summary>
/// Marker interface for retrieving user cards by multiple artists for convention signing planning.
/// Implements single-method delegation pattern with Execute method.
/// </summary>
internal interface IUserCardsForSigningDomainService
{
    Task<IOperationResponse<ISigningResultOufEntity>> Execute(
        IUserCardsForSigningItrEntity input,
        CancellationToken cancellationToken);
}
