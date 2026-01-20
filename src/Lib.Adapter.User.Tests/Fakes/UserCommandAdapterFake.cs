using System.Threading.Tasks;
using Lib.Adapter.User.Apis;
using Lib.Shared.DataModels.Entities.Itrs.User;
using Lib.Shared.DataModels.Entities.Oufs.User;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.User.Tests.Fakes;

internal sealed class UserCommandAdapterFake : IUserCommandAdapter
{
    public IOperationResponse<IUserSyncOufEntity> RegisterUserAsyncResult { get; init; }
    public int RegisterUserAsyncInvokeCount { get; private set; }

    public async Task<IOperationResponse<IUserSyncOufEntity>> RegisterUserAsync(IUserInfoItrEntity userInfo)
    {
        RegisterUserAsyncInvokeCount++;
        return await Task.FromResult(RegisterUserAsyncResult).ConfigureAwait(false);
    }
}
