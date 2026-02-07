using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.UserCards.Commands;

/// <summary>
/// Single-method domain service for adding a user card without updating UserSetCards.
/// Used by migration tools to separate individual card tracking from set-level aggregation.
/// </summary>
internal interface IAddUserCardOnlyDomainService
{
    Task<IOperationResponse<IUserCardOufEntity>> Execute(
        IUserCardItrEntity input,
        CancellationToken cancellationToken);
}
