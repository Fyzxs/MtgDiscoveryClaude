using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.User.Apis;

public sealed class UserAggregatorService : IUserAggregatorService
{
    private readonly UserInfoScribe _userInfoScribe;

    public UserAggregatorService(ILogger logger) : this(new UserInfoScribe(logger))
    { }

    private UserAggregatorService(UserInfoScribe userInfoScribe) => _userInfoScribe = userInfoScribe;

    public async Task<IOperationResponse<IUserInfoItrEntity>> RegisterUserAsync([NotNull] IUserInfoItrEntity userInfo)
    {
        UserInfoItem userItem = new()
        {
            UserId = userInfo.UserId,
            DisplayName = userInfo.UserNickname,
            SourceId = userInfo.UserSourceId
        };

        await _userInfoScribe.UpsertAsync(userItem).ConfigureAwait(false);

        return new SuccessOperationResponse<IUserInfoItrEntity>(userInfo);
    }

    Task<IOperationResponse<IUserInfoItrEntity>> IUserAggregatorService.RegisterUserAsync(IUserInfoItrEntity userInfo)
    {
        throw new System.NotImplementedException();
    }
}
