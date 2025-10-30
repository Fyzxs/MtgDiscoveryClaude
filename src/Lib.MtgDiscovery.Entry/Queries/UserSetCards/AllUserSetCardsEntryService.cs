using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Domain.UserSetCards.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSetCards;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.MtgDiscovery.Entry.Queries.Validators.UserSetCards;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries.UserSetCards;

internal sealed class AllUserSetCardsEntryService : IAllUserSetCardsEntryService
{
    private readonly IAllUserSetCardsArgEntityValidator _validator;
    private readonly IAllUserSetCardsArgToItrMapper _argToItrMapper;
    private readonly IUserSetCardsDomainService _domainService;
    private readonly ICollectionUserSetCardOufToOutMapper _oufToOutMapper;

    public AllUserSetCardsEntryService(ILogger logger) : this(
        new AllUserSetCardsArgEntityValidatorContainer(),
        new AllUserSetCardsArgToItrMapper(),
        new UserSetCardsDomainService(logger),
        new CollectionUserSetCardOufToOutMapper())
    {
    }

    private AllUserSetCardsEntryService(
        IAllUserSetCardsArgEntityValidator validator,
        IAllUserSetCardsArgToItrMapper argToItrMapper,
        IUserSetCardsDomainService domainService,
        ICollectionUserSetCardOufToOutMapper oufToOutMapper)
    {
        _validator = validator;
        _argToItrMapper = argToItrMapper;
        _domainService = domainService;
        _oufToOutMapper = oufToOutMapper;
    }

    public async Task<IOperationResponse<List<UserSetCardOutEntity>>> Execute(IAllUserSetCardsArgEntity userSetCardsArgs)
    {
        IValidatorActionResult<IOperationResponse<IEnumerable<IUserSetCardOufEntity>>> validatorResult =
            await _validator.Validate(userSetCardsArgs).ConfigureAwait(false);

        if (validatorResult.IsNotValid())
        {
            return new FailureOperationResponse<List<UserSetCardOutEntity>>(
                validatorResult.FailureStatus().OuterException);
        }

        IAllUserSetCardsItrEntity itrEntity =
            await _argToItrMapper.Map(userSetCardsArgs).ConfigureAwait(false);

        IOperationResponse<IEnumerable<IUserSetCardOufEntity>> opResponse =
            await _domainService.AllUserSetCardsAsync(itrEntity).ConfigureAwait(false);

        if (opResponse.IsFailure)
        {
            return new FailureOperationResponse<List<UserSetCardOutEntity>>(
                opResponse.OuterException);
        }

        List<UserSetCardOutEntity> outEntities =
            await _oufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);

        return new SuccessOperationResponse<List<UserSetCardOutEntity>>(outEntities);
    }
}
