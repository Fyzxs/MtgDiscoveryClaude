using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Domain.UserCards.Apis;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.MtgDiscovery.Entry.Queries.Validators.UserCards;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries.UserCards;

internal sealed class UserCardsBySetEntryService : IUserCardsBySetEntryService
{
    private readonly IUserCardsDomainService _userCardsDomainService;
    private readonly IUserCardsSetArgEntityValidator _userCardsSetArgEntityValidator;
    private readonly IUserCardsSetArgToItrMapper _userCardsSetArgToItrMapper;
    private readonly ICollectionUserCardOufToOutMapper _userCardOufToOutMapper;

    public UserCardsBySetEntryService(ILogger logger) : this(
        new UserCardsDomainService(logger),
        new UserCardsSetArgEntityValidatorContainer(),
        new UserCardsSetArgToItrMapper(),
        new CollectionUserCardOufToOutMapper())
    { }

    private UserCardsBySetEntryService(
        IUserCardsDomainService userCardsDomainService,
        IUserCardsSetArgEntityValidator userCardsSetArgEntityValidator,
        IUserCardsSetArgToItrMapper userCardsSetArgToItrMapper,
        ICollectionUserCardOufToOutMapper userCardOufToOutMapper)
    {
        _userCardsDomainService = userCardsDomainService;
        _userCardsSetArgEntityValidator = userCardsSetArgEntityValidator;
        _userCardsSetArgToItrMapper = userCardsSetArgToItrMapper;
        _userCardOufToOutMapper = userCardOufToOutMapper;
    }

    public async Task<IOperationResponse<List<UserCardOutEntity>>> Execute(IUserCardsBySetArgEntity bySetArgs)
    {
        IValidatorActionResult<IOperationResponse<IEnumerable<IUserCardOufEntity>>> validatorResult = await _userCardsSetArgEntityValidator.Validate(bySetArgs).ConfigureAwait(false);
        if (validatorResult.IsNotValid()) return new FailureOperationResponse<List<UserCardOutEntity>>(validatorResult.FailureStatus().OuterException);

        IUserCardsSetItrEntity itrEntity = await _userCardsSetArgToItrMapper.Map(bySetArgs).ConfigureAwait(false);
        IOperationResponse<IEnumerable<IUserCardOufEntity>> opResponse = await _userCardsDomainService.UserCardsBySetAsync(itrEntity).ConfigureAwait(false);
        if (opResponse.IsFailure) return new FailureOperationResponse<List<UserCardOutEntity>>(opResponse.OuterException);

        List<UserCardOutEntity> outEntities = await _userCardOufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<List<UserCardOutEntity>>(outEntities);
    }
}
