using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.UserCards.Commands;

/// <summary>
/// Marker interface for adding a user card to the collection.
/// Implements single-method delegation pattern with Execute method.
/// </summary>
internal interface IAddUserCardDomainService
{
    Task<IOperationResponse<IUserCardOufEntity>> Execute(
        IUserCardItrEntity input,
        CancellationToken cancellationToken);
}
