using System.Threading.Tasks;
using Lib.Domain.User.Commands;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.User.Apis;

public sealed class UserDomainService : IUserDomainService
{
    private readonly IUserCommandDomainService _userDomainOperations;

    public UserDomainService(ILogger logger) : this(new UserCommandDomainService(logger))
    { }

    private UserDomainService(IUserCommandDomainService userDomainOperations) => _userDomainOperations = userDomainOperations;

    public Task<IOperationResponse<IUserInfoOufEntity>> RegisterUserAsync(IUserInfoItrEntity userInfo) => _userDomainOperations.RegisterUserAsync(userInfo);
}
