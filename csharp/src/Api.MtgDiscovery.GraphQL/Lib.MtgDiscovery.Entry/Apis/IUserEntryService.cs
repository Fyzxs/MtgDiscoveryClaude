using System.Threading;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.User;
using Lib.Shared.DataModels.Entities.Args.User;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Apis;

public interface IUserEntryService
{
    Task<IOperationResponse<UserSyncOutEntity>> RegisterUserAsync(IAuthUserArgEntity authUser, CancellationToken cancellationToken);
}
