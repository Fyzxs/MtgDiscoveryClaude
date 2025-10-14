using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.User;
using Lib.MtgDiscovery.Entry.Queries.User;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries;

internal sealed class UserEntryService : IUserEntryService
{
    private readonly IRegisterUserEntryService _registerUser;

    public UserEntryService(ILogger logger) : this(
        new RegisterUserEntryService(logger))
    { }

    private UserEntryService(IRegisterUserEntryService registerUser) => _registerUser = registerUser;

    public async Task<IOperationResponse<UserRegistrationOutEntity>> RegisterUserAsync(IAuthUserArgEntity authUser) => await _registerUser.Execute(authUser).ConfigureAwait(false);
}
