using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.User.Apis;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.User.Tests.Fakes;

internal sealed class UserCommandAdapterFake : IUserCommandAdapter
{
    public IOperationResponse<UserInfoExtEntity> RegisterUserAsyncResult { get; init; }
    public int RegisterUserAsyncInvokeCount { get; private set; }

    public async Task<IOperationResponse<UserInfoExtEntity>> RegisterUserAsync(IUserInfoItrEntity userInfo)
    {
        RegisterUserAsyncInvokeCount++;
        return await Task.FromResult(RegisterUserAsyncResult).ConfigureAwait(false);
    }
}
