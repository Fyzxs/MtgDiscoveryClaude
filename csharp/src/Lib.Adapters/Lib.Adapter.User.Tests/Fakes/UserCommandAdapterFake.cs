using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserInfo;
using Lib.Adapter.User.Apis;
using Lib.Adapter.User.Apis.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.User.Tests.Fakes;

internal sealed class UserCommandAdapterFake : IUserCommandAdapter
{
    public IOperationResponse<UserInfoExtEntity> RegisterUserAsyncResult { get; init; }
    public int RegisterUserAsyncInvokeCount { get; private set; }

    public async Task<IOperationResponse<UserInfoExtEntity>> RegisterUserAsync(IUserInfoXfrEntity userInfo, CancellationToken cancellationToken)
    {
        RegisterUserAsyncInvokeCount++;
        return await Task.FromResult(RegisterUserAsyncResult).ConfigureAwait(false);
    }
}
