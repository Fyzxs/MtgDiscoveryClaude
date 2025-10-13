using System.Threading.Tasks;
using Lib.Domain.User.Apis;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.MtgDiscovery.Entry.Queries.Validators.Users;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.MtgDiscovery.Entry.Entities.Outs.User;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries.User;

internal sealed class RegisterUserEntryService : IRegisterUserEntryService
{
    private readonly IUserDomainService _userDomainService;
    private readonly IAuthUserArgEntityValidator _authUserArgEntityValidator;
    private readonly IAuthUserArgToItrMapper _authUserArgToItrMapper;
    private readonly IUserInfoOufToOutMapper _userInfoOufToOutMapper;

    public RegisterUserEntryService(ILogger logger) : this(
        new UserDomainService(logger),
        new AuthUserArgEntityValidatorContainer(),
        new AuthUserArgToItrMapper(),
        new UserInfoOufToOutMapper())
    { }

    private RegisterUserEntryService(
        IUserDomainService userDomainService,
        IAuthUserArgEntityValidator authUserArgEntityValidator,
        IAuthUserArgToItrMapper authUserArgToItrMapper,
        IUserInfoOufToOutMapper userInfoOufToOutMapper)
    {
        _userDomainService = userDomainService;
        _authUserArgEntityValidator = authUserArgEntityValidator;
        _authUserArgToItrMapper = authUserArgToItrMapper;
        _userInfoOufToOutMapper = userInfoOufToOutMapper;
    }

    public async Task<IOperationResponse<UserRegistrationOutEntity>> Execute(IAuthUserArgEntity authUser)
    {
        IValidatorActionResult<IOperationResponse<IUserRegistrationItrEntity>> validatorResult = await _authUserArgEntityValidator.Validate(authUser).ConfigureAwait(false);
        if (validatorResult.IsNotValid()) return new FailureOperationResponse<UserRegistrationOutEntity>(validatorResult.FailureStatus().OuterException);

        IUserInfoItrEntity itrEntity = await _authUserArgToItrMapper.Map(authUser).ConfigureAwait(false);
        IOperationResponse<IUserInfoOufEntity> opResponse = await _userDomainService.RegisterUserAsync(itrEntity).ConfigureAwait(false);
        if (opResponse.IsFailure) return new FailureOperationResponse<UserRegistrationOutEntity>(opResponse.OuterException);

        UserRegistrationOutEntity outEntity = await _userInfoOufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<UserRegistrationOutEntity>(outEntity);
    }
}
