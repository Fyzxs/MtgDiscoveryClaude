using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserInfo;
using Lib.Adapter.User.Apis;
using Lib.Adapter.User.Apis.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.User.Commands;

/// <summary>
/// Cosmos DB implementation of the user command adapter.
///
/// This class coordinates all Cosmos DB-specific user command operations
/// by delegating to specialized single-method adapters.
/// The main UserAdapterService delegates to this implementation.
/// </summary>
internal sealed class UserCommandAdapter : IUserCommandAdapter
{
    private readonly IRegisterUserAdapter _registerUserAdapter;

    public UserCommandAdapter(ILogger logger) : this(new RegisterUserAdapter(logger)) { }

    private UserCommandAdapter(IRegisterUserAdapter registerUserAdapter) => _registerUserAdapter = registerUserAdapter;

    public async Task<IOperationResponse<UserInfoExtEntity>> RegisterUserAsync(IUserInfoXfrEntity userInfo, CancellationToken cancellationToken) => await _registerUserAdapter.Execute(userInfo, cancellationToken).ConfigureAwait(false);
}
