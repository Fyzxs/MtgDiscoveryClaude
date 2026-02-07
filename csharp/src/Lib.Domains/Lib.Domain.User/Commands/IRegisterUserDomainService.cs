using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.User;
using Lib.Shared.DataModels.Entities.Oufs.User;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.User.Commands;

/// <summary>
/// Marker interface for user registration/sync operation.
/// Implements single-method delegation pattern with Execute method.
/// Returns IUserSyncOufEntity with isFirstLogin flag.
/// </summary>
internal interface IRegisterUserDomainService
{
    Task<IOperationResponse<IUserSyncOufEntity>> Execute(IUserInfoItrEntity input, CancellationToken cancellationToken);
}
