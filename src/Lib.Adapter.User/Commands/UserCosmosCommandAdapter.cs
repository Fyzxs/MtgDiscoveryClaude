using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Adapter.User.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.User.Commands;

/// <summary>
/// Cosmos DB implementation of the user persistence adapter.
/// 
/// This class handles all Cosmos DB-specific user persistence operations,
/// implementing the specialized IUserPersistenceAdapter interface.
/// The main UserAdapterService delegates to this implementation.
/// </summary>
internal sealed class UserCosmosCommandAdapter : IUserCommandAdapter
{
    private readonly UserInfoScribe _userInfoScribe;

    public UserCosmosCommandAdapter(ILogger logger) : this(new UserInfoScribe(logger))
    { }

    private UserCosmosCommandAdapter(UserInfoScribe userInfoScribe) => _userInfoScribe = userInfoScribe;

    public async Task<IOperationResponse<UserInfoExtEntity>> RegisterUserAsync([NotNull] IUserInfoItrEntity userInfo)
    {
        // Extract primitives for external system interface and map to storage entity
        UserInfoExtEntity userItem = new()
        {
            UserId = userInfo.UserId,
            DisplayName = userInfo.UserNickname,
            SourceId = userInfo.UserSourceId
        };

        await _userInfoScribe.UpsertAsync(userItem).ConfigureAwait(false);

        return new SuccessOperationResponse<UserInfoExtEntity>(userItem);
    }
}
