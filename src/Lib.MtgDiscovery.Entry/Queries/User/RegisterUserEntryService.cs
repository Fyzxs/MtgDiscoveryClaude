using System.Threading.Tasks;
using Lib.Domain.User.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.User;
using Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;
using Lib.MtgDiscovery.Entry.Queries.Actions.Validators.Users;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.User;
using Lib.Shared.DataModels.Entities.Itrs.User;
using Lib.Shared.DataModels.Entities.Oufs.User;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries.User;

internal sealed class RegisterUserEntryService : IRegisterUserEntryService
{
    private readonly IUserDomainService _userDomainService;
    private readonly IAuthUserArgEntityValidator _authUserArgEntityValidator;
    private readonly IAuthUserArgToItrMapper _authUserArgToItrMapper;
    private readonly IUserSyncOufToOutMapper _userSyncOufToOutMapper;

    public RegisterUserEntryService(ILogger logger) : this(
        new UserDomainService(logger),
        new AuthUserArgEntityValidatorContainer(),
        new AuthUserArgToItrMapper(),
        new UserSyncOufToOutMapper())
    { }

    private RegisterUserEntryService(
        IUserDomainService userDomainService,
        IAuthUserArgEntityValidator authUserArgEntityValidator,
        IAuthUserArgToItrMapper authUserArgToItrMapper,
        IUserSyncOufToOutMapper userSyncOufToOutMapper)
    {
        _userDomainService = userDomainService;
        _authUserArgEntityValidator = authUserArgEntityValidator;
        _authUserArgToItrMapper = authUserArgToItrMapper;
        _userSyncOufToOutMapper = userSyncOufToOutMapper;
    }

    public async Task<IOperationResponse<UserSyncOutEntity>> Execute(IAuthUserArgEntity authUser)
    {
        IValidatorActionResult<IOperationResponse<IUserRegistrationItrEntity>> validatorResult = await _authUserArgEntityValidator.Validate(authUser).ConfigureAwait(false);
        if (validatorResult.IsNotValid())
        {
            return new FailureOperationResponse<UserSyncOutEntity>(validatorResult.FailureStatus().OuterException);
        }

        IUserInfoItrEntity itrEntity = await _authUserArgToItrMapper.Map(authUser).ConfigureAwait(false);
        IOperationResponse<IUserSyncOufEntity> opResponse = await _userDomainService.RegisterUserAsync(itrEntity).ConfigureAwait(false);
        if (opResponse.IsFailure)
        {
            return new FailureOperationResponse<UserSyncOutEntity>(opResponse.OuterException);
        }

        UserSyncOutEntity outEntity = await _userSyncOufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<UserSyncOutEntity>(outEntity);
    }
}
