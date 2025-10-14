using System.Threading.Tasks;
using Lib.Domain.UserSetCards.Apis;
using Lib.MtgDiscovery.Entry.Commands.UserSetCards.Mappers;
using Lib.MtgDiscovery.Entry.Commands.UserSetCards.Validators;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSetCards;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Args.UserSetCards;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Commands.UserSetCards;

internal sealed class AddSetGroupToUserSetCardEntryService : IAddSetGroupToUserSetCardEntryService
{
    private readonly IUserSetCardsDomainService _userSetCardsDomainService;
    private readonly IAddSetGroupCombinedArgToItrMapper _argToItrMapper;
    private readonly IUserSetCardOufToOutMapper _oufToOutMapper;
    private readonly IAddSetGroupToUserSetCardArgsValidator _validator;

    public AddSetGroupToUserSetCardEntryService(ILogger logger) : this(
        new UserSetCardsDomainService(logger),
        new AddSetGroupCombinedArgToItrMapper(),
        new UserSetCardOufToOutMapper(),
        new AddSetGroupToUserSetCardArgsValidatorContainer())
    { }

    private AddSetGroupToUserSetCardEntryService(
        IUserSetCardsDomainService userSetCardsDomainService,
        IAddSetGroupCombinedArgToItrMapper argToItrMapper,
        IUserSetCardOufToOutMapper oufToOutMapper,
        IAddSetGroupToUserSetCardArgsValidator validator)
    {
        _userSetCardsDomainService = userSetCardsDomainService;
        _argToItrMapper = argToItrMapper;
        _oufToOutMapper = oufToOutMapper;
        _validator = validator;
    }

    public async Task<IOperationResponse<UserSetCardOutEntity>> Execute(IAddSetGroupToUserSetCardArgsEntity argsEntity)
    {

        IValidatorActionResult<IOperationResponse<IUserSetCardOufEntity>> validationResult = await _validator.Validate(argsEntity).ConfigureAwait(false);
        if (validationResult.IsNotValid()) return new FailureOperationResponse<UserSetCardOutEntity>(validationResult.FailureStatus().OuterException);

        IAddSetGroupToUserSetCardItrEntity itrEntity = await _argToItrMapper.Map(argsEntity).ConfigureAwait(false);

        IOperationResponse<IUserSetCardOufEntity> domainResponse = await _userSetCardsDomainService.AddSetGroupToUserSetCardAsync(itrEntity).ConfigureAwait(false);
        if (domainResponse.IsFailure) return new FailureOperationResponse<UserSetCardOutEntity>(domainResponse.OuterException);

        UserSetCardOutEntity outEntity = await _oufToOutMapper.Map(domainResponse.ResponseData).ConfigureAwait(false);

        return new SuccessOperationResponse<UserSetCardOutEntity>(outEntity);
    }
}
