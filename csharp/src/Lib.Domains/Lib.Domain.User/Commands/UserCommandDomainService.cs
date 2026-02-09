using System.Threading;
using System.Threading.Tasks;
using Lib.Domain.User.Apis;
using Lib.Shared.DataModels.Entities.Itrs.User;
using Lib.Shared.DataModels.Entities.Oufs.User;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.User.Commands;

internal sealed class UserCommandDomainService : IUserCommandDomainService
{
    private readonly IRegisterUserDomain _registerUserService;

    public UserCommandDomainService(ILogger logger) : this(new RegisterUserDomain(logger))
    { }

    private UserCommandDomainService(IRegisterUserDomain registerUserService) => _registerUserService = registerUserService;

    public async Task<IOperationResponse<IUserSyncOufEntity>> RegisterUserAsync(IUserInfoItrEntity userInfo, CancellationToken cancellationToken) => await _registerUserService.Execute(userInfo, cancellationToken).ConfigureAwait(false);
}
