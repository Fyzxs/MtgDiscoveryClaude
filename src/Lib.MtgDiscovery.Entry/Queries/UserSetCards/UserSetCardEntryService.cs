using System.Threading.Tasks;
using Lib.Aggregator.UserSetCards.Entities;
using Lib.Domain.UserSetCards.Apis;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.MtgDiscovery.Entry.Queries.Validators.UserSetCards;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Outs.UserSetCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries.UserSetCards;

internal sealed class UserSetCardEntryService : IUserSetCardEntryService
{
    private readonly IUserSetCardsDomainService _userSetCardsDomainService;
    private readonly IUserSetCardArgEntityValidator _userSetCardArgEntityValidator;
    private readonly IUserSetCardArgToItrMapper _userSetCardArgToItrMapper;
    private readonly IUserSetCardOufToOutMapper _userSetCardOufToOutMapper;

    public UserSetCardEntryService(ILogger logger) : this(
        new UserSetCardsDomainService(logger),
        new UserSetCardArgEntityValidatorContainer(),
        new UserSetCardArgToItrMapper(),
        new UserSetCardOufToOutMapper())
    { }

    private UserSetCardEntryService(
        IUserSetCardsDomainService userSetCardsDomainService,
        IUserSetCardArgEntityValidator userSetCardArgEntityValidator,
        IUserSetCardArgToItrMapper userSetCardArgToItrMapper,
        IUserSetCardOufToOutMapper userSetCardOufToOutMapper)
    {
        _userSetCardsDomainService = userSetCardsDomainService;
        _userSetCardArgEntityValidator = userSetCardArgEntityValidator;
        _userSetCardArgToItrMapper = userSetCardArgToItrMapper;
        _userSetCardOufToOutMapper = userSetCardOufToOutMapper;
    }

    public async Task<IOperationResponse<UserSetCardOutEntity>> Execute(IUserSetCardArgEntity userSetCardArgs)
    {
        IValidatorActionResult<IOperationResponse<IUserSetCardOufEntity>> validatorResult = await _userSetCardArgEntityValidator.Validate(userSetCardArgs).ConfigureAwait(false);
        if (validatorResult.IsNotValid()) return new FailureOperationResponse<UserSetCardOutEntity>(validatorResult.FailureStatus().OuterException);

        IUserSetCardItrEntity itrEntity = await _userSetCardArgToItrMapper.Map(userSetCardArgs).ConfigureAwait(false);
        IOperationResponse<IUserSetCardOufEntity> opResponse = await _userSetCardsDomainService.GetUserSetCardByUserAndSetAsync(itrEntity).ConfigureAwait(false);
        if (opResponse.IsFailure) return new FailureOperationResponse<UserSetCardOutEntity>(opResponse.OuterException);

        UserSetCardOutEntity outEntity = await _userSetCardOufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<UserSetCardOutEntity>(outEntity);
    }
}
