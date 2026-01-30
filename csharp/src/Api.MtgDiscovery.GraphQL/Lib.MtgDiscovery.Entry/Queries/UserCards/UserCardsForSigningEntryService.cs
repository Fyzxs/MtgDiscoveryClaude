using System.Threading.Tasks;
using Lib.Domain.UserCards.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.Signing;
using Lib.MtgDiscovery.Entry.Queries.Actions.Mappers.Signing;
using Lib.MtgDiscovery.Entry.Queries.Actions.Validators.UserCards;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.UserCards;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards.Signing;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries.UserCards;

internal sealed class UserCardsForSigningEntryService : IUserCardsForSigningEntryService
{
    private readonly IUserCardsDomainService _userCardsDomainService;
    private readonly IUserCardsForSigningArgEntityValidator _validator;
    private readonly IUserCardsForSigningArgToItrMapper _argToItrMapper;
    private readonly ISigningResultOufToOutMapper _oufToOutMapper;

    public UserCardsForSigningEntryService(ILogger logger) : this(
        new UserCardsDomainService(logger),
        new UserCardsForSigningArgEntityValidatorContainer(),
        new UserCardsForSigningArgToItrMapper(),
        new SigningResultOufToOutMapper())
    { }

    private UserCardsForSigningEntryService(
        IUserCardsDomainService userCardsDomainService,
        IUserCardsForSigningArgEntityValidator validator,
        IUserCardsForSigningArgToItrMapper argToItrMapper,
        ISigningResultOufToOutMapper oufToOutMapper)
    {
        _userCardsDomainService = userCardsDomainService;
        _validator = validator;
        _argToItrMapper = argToItrMapper;
        _oufToOutMapper = oufToOutMapper;
    }

    public async Task<IOperationResponse<SigningResultOutEntity>> Execute(IUserCardsForSigningArgEntity argEntity)
    {
        IValidatorActionResult<IOperationResponse<ISigningResultOufEntity>> validatorResult = await _validator.Validate(argEntity).ConfigureAwait(false);
        if (validatorResult.IsNotValid())
        {
            return new FailureOperationResponse<SigningResultOutEntity>(validatorResult.FailureStatus().OuterException);
        }

        IUserCardsForSigningItrEntity itrEntity = await _argToItrMapper.Map(argEntity).ConfigureAwait(false);
        IOperationResponse<ISigningResultOufEntity> opResponse = await _userCardsDomainService.UserCardsForSigningAsync(itrEntity).ConfigureAwait(false);
        if (opResponse.IsFailure)
        {
            return new FailureOperationResponse<SigningResultOutEntity>(opResponse.OuterException);
        }

        // No enrichment needed - data is denormalized in UserCards collection
        SigningResultOutEntity outEntity = await _oufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<SigningResultOutEntity>(outEntity);
    }
}
