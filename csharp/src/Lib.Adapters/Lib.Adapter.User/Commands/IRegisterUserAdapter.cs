using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.User;
using Lib.Shared.DataModels.Entities.Oufs.User;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.User.Commands;

/// <summary>
/// Adapter for registering or syncing user information in storage.
/// Returns isFirstLogin flag to indicate new vs returning user.
/// </summary>
internal interface IRegisterUserAdapter
{
    Task<IOperationResponse<IUserSyncOufEntity>> Execute(IUserInfoItrEntity input, CancellationToken cancellationToken);
}
