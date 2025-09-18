using System.Threading.Tasks;
using Lib.Domain.User.Apis;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.MtgDiscovery.Entry.Queries.Validators.Users;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries;

internal sealed class UserEntryService : IUserEntryService
{
    private readonly IUserDomainService _userDomainService;
    private readonly IAuthUserArgEntityValidator _authUserValidator;
    private readonly IAuthUserArgsToItrMapper _authUserMapper;

    public UserEntryService(ILogger logger) : this(
        new UserDomainService(logger),
        new AuthUserArgEntityValidatorContainer(),
        new AuthUserArgsToItrMapper())
    { }

    private UserEntryService(
        IUserDomainService userDataService,
        IAuthUserArgEntityValidator authUserValidator,
        IAuthUserArgsToItrMapper authUserMapper)
    {
        _userDomainService = userDataService;
        _authUserValidator = authUserValidator;
        _authUserMapper = authUserMapper;
    }

    public async Task<IOperationResponse<IUserInfoOufEntity>> RegisterUserAsync(IAuthUserArgEntity authUser)
    {
        IValidatorActionResult<IOperationResponse<IUserRegistrationItrEntity>> result = await _authUserValidator.Validate(authUser).ConfigureAwait(false);

        if (result.IsNotValid()) return new FailureOperationResponse<IUserInfoOufEntity>(result.FailureStatus().OuterException);

        IUserInfoItrEntity mappedArgs = await _authUserMapper.Map(authUser).ConfigureAwait(false);
        return await _userDomainService.RegisterUserAsync(mappedArgs).ConfigureAwait(false);
    }
}
