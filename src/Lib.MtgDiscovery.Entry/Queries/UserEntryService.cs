using System.Threading.Tasks;
using Lib.Domain.User.Apis;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.MtgDiscovery.Entry.Queries.Validators.Users;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.DataModels.Entities.Outs.User;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries;

internal sealed class UserEntryService : IUserEntryService
{
    private readonly IUserDomainService _userDomainService;
    private readonly IAuthUserArgEntityValidator _authUserArgEntityValidator;
    private readonly IAuthUserArgsToItrMapper _authUserArgsToItrMapper;
    private readonly IUserInfoOufToOutMapper _userInfoOufToOutMapper;

    public UserEntryService(ILogger logger) : this(
        new UserDomainService(logger),
        new AuthUserArgEntityValidatorContainer(),
        new AuthUserArgsToItrMapper(),
        new UserInfoOufToOutMapper())
    { }

    private UserEntryService(
        IUserDomainService userDomainService,
        IAuthUserArgEntityValidator authUserArgEntityValidator,
        IAuthUserArgsToItrMapper authUserArgsToItrMapper,
        IUserInfoOufToOutMapper userInfoOufToOutMapper)
    {
        _userDomainService = userDomainService;
        _authUserArgEntityValidator = authUserArgEntityValidator;
        _authUserArgsToItrMapper = authUserArgsToItrMapper;
        _userInfoOufToOutMapper = userInfoOufToOutMapper;
    }

    public async Task<IOperationResponse<UserRegistrationOutEntity>> RegisterUserAsync(IAuthUserArgEntity authUser)
    {
        IValidatorActionResult<IOperationResponse<IUserRegistrationItrEntity>> validatorResult = await _authUserArgEntityValidator.Validate(authUser).ConfigureAwait(false);
        if (validatorResult.IsNotValid()) return new FailureOperationResponse<UserRegistrationOutEntity>(validatorResult.FailureStatus().OuterException);

        IUserInfoItrEntity itrEntity = await _authUserArgsToItrMapper.Map(authUser).ConfigureAwait(false);
        IOperationResponse<IUserInfoOufEntity> opResponse = await _userDomainService.RegisterUserAsync(itrEntity).ConfigureAwait(false);
        if (opResponse.IsFailure) return new FailureOperationResponse<UserRegistrationOutEntity>(opResponse.OuterException);

        UserRegistrationOutEntity outEntity = await _userInfoOufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<UserRegistrationOutEntity>(outEntity);
    }
}
