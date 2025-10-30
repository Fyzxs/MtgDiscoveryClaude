using System.Threading.Tasks;
using Lib.Aggregator.User.Apis;
using Lib.Domain.User.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.User.Commands;

internal sealed class UserCommandDomainService : IUserCommandDomainService
{
    private readonly IRegisterUserDomainService _registerUserService;

    public UserCommandDomainService(ILogger logger) : this(new RegisterUserDomainService(logger))
    { }

    private UserCommandDomainService(IRegisterUserDomainService registerUserService) => _registerUserService = registerUserService;

    public async Task<IOperationResponse<IUserInfoOufEntity>> RegisterUserAsync(IUserInfoItrEntity userInfo)
        => await _registerUserService.Execute(userInfo).ConfigureAwait(false);
}
