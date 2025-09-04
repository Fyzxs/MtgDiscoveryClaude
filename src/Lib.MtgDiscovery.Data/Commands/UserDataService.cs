using System.Threading.Tasks;
using Lib.Domain.User.Apis;
using Lib.Shared.DataModels.Entities;
using Lib.MtgDiscovery.Data.Apis;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Data.Commands;

internal sealed class UserDataService : IUserDataService
{
    private readonly IUserDomainService _userDomainService;

    public UserDataService(ILogger logger) : this(new UserDomainService(logger))
    {
    }

    private UserDataService(IUserDomainService userDomainService)
    {
        _userDomainService = userDomainService;
    }

    public async Task<IOperationResponse<IUserRegistrationItrEntity>> RegisterUserAsync(IUserInfoItrEntity userInfo)
    {
        return await _userDomainService.RegisterUserAsync(userInfo).ConfigureAwait(false);
    }
}