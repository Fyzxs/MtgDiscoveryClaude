using System.Threading.Tasks;
using Lib.MtgDiscovery.Data.Apis;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.MtgDiscovery.Entry.Queries.Validators;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries;

internal sealed class UserEntryService : IUserEntryService
{
    private readonly IUserDataService _userDataService;
    private readonly IAuthUserArgEntityValidator _authUserValidator;
    private readonly IAuthUserArgsToItrMapper _authUserMapper;

    public UserEntryService(ILogger logger) : this(
        new DataService(logger),
        new AuthUserArgEntityValidatorContainer(),
        new AuthUserArgsToItrMapper())
    { }

    private UserEntryService(
        IUserDataService userDataService,
        IAuthUserArgEntityValidator authUserValidator,
        IAuthUserArgsToItrMapper authUserMapper)
    {
        _userDataService = userDataService;
        _authUserValidator = authUserValidator;
        _authUserMapper = authUserMapper;
    }

    public async Task<IOperationResponse<IUserInfoItrEntity>> RegisterUserAsync(IAuthUserArgEntity authUser)
    {
        IValidatorActionResult<IOperationResponse<IUserRegistrationItrEntity>> result = await _authUserValidator.Validate(authUser).ConfigureAwait(false);

        if (result.IsNotValid()) return new FailureOperationResponse<IUserInfoItrEntity>(result.FailureStatus().OuterException);

        IUserInfoItrEntity mappedArgs = await _authUserMapper.Map(authUser).ConfigureAwait(false);
        return await _userDataService.RegisterUserAsync(mappedArgs).ConfigureAwait(false);
    }
}
