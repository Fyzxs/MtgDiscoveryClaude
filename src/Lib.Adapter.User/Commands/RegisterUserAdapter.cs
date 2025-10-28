using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.User.Commands;

/// <summary>
/// Registers new user information in Cosmos DB storage.
/// </summary>
internal sealed class RegisterUserAdapter : IRegisterUserAdapter
{
    private readonly UserInfoScribe _userInfoScribe;

    public RegisterUserAdapter(ILogger logger) : this(new UserInfoScribe(logger)) { }

    private RegisterUserAdapter(UserInfoScribe userInfoScribe) => _userInfoScribe = userInfoScribe;

    public async Task<IOperationResponse<UserInfoExtEntity>> Execute([NotNull] IUserInfoItrEntity input)
    {
        // Extract primitives for external system interface and map to storage entity
        UserInfoExtEntity userItem = new()
        {
            UserId = input.UserId,
            DisplayName = input.UserNickname,
            SourceId = input.UserSourceId
        };

        await _userInfoScribe.UpsertAsync(userItem).ConfigureAwait(false);

        return new SuccessOperationResponse<UserInfoExtEntity>(userItem);
    }
}
